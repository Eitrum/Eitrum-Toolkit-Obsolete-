using System;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace Eitrum
{
	public class EiThreadedUpdateSystem : EiCoreSingleton<EiThreadedUpdateSystem>
	{
		#region Singleton Creation

		public override void SingletonCreation ()
		{
			var us = EiUpdateSystem.Instance;
			threads = new ThreadContainer[Math.Max (1, Environment.ProcessorCount - 1)];
			for (int i = 0; i < threads.Length; i++) {
				threads [i] = new ThreadContainer (1000);
			}
			var inst = EiUnityThreading.Instance;
			EiUnityThreading.CloseThreads.SubscribeThreadSafe (CloseThreads);
		}

		#endregion

		#region Classes

		public class ThreadContainer : EiCore
		{
			#region Variables

			Thread thread;
			EiLinkedList<EiUpdateInterface> components = new EiLinkedList<EiUpdateInterface> ();

			long targetFpsInTicks = 0;
			long ticks = 0;
			long updates = 0;
			long framesPerSecond = 0;
			long lastFpsUpdate = 0;
			Stopwatch sw = new Stopwatch ();

			bool isRunning = true;

			#endregion

			#region Properties

			public long UpdatesPerSecond {
				get {
					return framesPerSecond;
				}
			}

			public float DeltaTime {
				get {
					return (((float)ticks) / (float)TimeSpan.TicksPerSecond);
				}
			}

			public bool IsRunning {
				set {
					if (!isRunning && value)
						Instantiate (targetFpsInTicks);
					isRunning = value;
				}get {
					return isRunning || thread.IsAlive; 
				}
			}

			#endregion

			#region Constructors

			public ThreadContainer (int fpsLimit = 10000)
			{
				Instantiate (TimeSpan.TicksPerSecond / fpsLimit);
			}

			#endregion

			#region Core

			public void Instantiate (long targetFpsInTicks = 1000)
			{
				this.targetFpsInTicks = targetFpsInTicks;
				thread = new Thread (new ThreadStart (Update));
				thread.IsBackground = true;
				thread.Start ();
			}

			void Update ()
			{
				while (isRunning) {

					#region start
					sw.Start ();

					var time = DeltaTime;
					updates++;

					#endregion

					#region EiComponent Update

					try {
						EiLLNode<EiUpdateInterface> component;
						var iterator = components.GetIterator ();
						while (iterator.Next (out component)) {
							try {
								if (component.Value == null) {
									components.Remove (component);
								} else {
									component.Value.ThreadedUpdateComponent (time);
								}
							} catch (Exception e) {
								if (component != null)
									components.Remove (component);
								LogException (() => e);
							}
						}
					} catch (Exception e) {
						LogException (() => e);
					}

					#endregion

					#region FPS Counter

					if (DateTime.UtcNow.Ticks > lastFpsUpdate + TimeSpan.TicksPerSecond) {
						lastFpsUpdate = DateTime.UtcNow.Ticks;
						framesPerSecond = updates;
						updates = 0;
					}
					#endregion

					#region End and sleep

					Thread.Sleep (new TimeSpan (Math.Max (1000, targetFpsInTicks - ticks)));
					ticks = sw.ElapsedTicks;
					sw.Reset ();

					#endregion
				}
			}

			public EiLLNode<EiUpdateInterface> Subscribe (EiUpdateInterface component)
			{
				return components.Add (component);
			}

			public void Unsubscribe (EiLLNode<EiUpdateInterface> node)
			{
				components.Remove (node);
			}

			#endregion
		}


		#endregion

		#region Variables

		public ThreadContainer[] threads;

		#endregion

		#region Core

		public void CloseThreads (bool value = true)
		{
			if (value) {
				for (int i = 0; i < threads.Length; i++) {
					threads [i].IsRunning = false;
				}
			}
		}

		public ThreadContainer this [int index] {
			get {
				if (index < 0 || index >= threads.Length)
					return null;
				return threads [index];
			}
		}

		#endregion

		#region Subscribe / Unsubscribe

		public EiLLNode<EiUpdateInterface> Subscribe (EiUpdateInterface component)
		{
			int index = 0;
			long fastest = 0;
			for (int i = 0; i < threads.Length; i++) {
				if (threads [i].UpdatesPerSecond >= fastest) {
					index = i;
					fastest = threads [i].UpdatesPerSecond;
				}
			}
			return threads [index].Subscribe (component);
		}

		public void Unsubscribe (EiLLNode<EiUpdateInterface> node)
		{
			if (node.List != null)
				node.List.Remove (node);
			else
				LogError (() => "Cant remove node from system");
		}

		#endregion
	}
}

