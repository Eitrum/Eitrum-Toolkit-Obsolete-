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
			var processors = Math.Max (1, Environment.ProcessorCount - 1);
			for (int i = 0; i < processors; i++) {
				var thread = new ThreadContainer (10000);
				var node = threads.Add (thread);
				thread.SetNode (node);
			}
			var inst = EiUnityThreading.Instance;
			EiUnityThreading.CloseThreads.SubscribeThreadSafe (CloseThreads);
		}

		#endregion

		#region Classes

		public class ThreadContainer : EiCore
		{
			#region Variables

			EiLLNode<ThreadContainer> node;
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

			public void SetNode (EiLLNode<ThreadContainer> node)
			{
				this.node = node;
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

					if (components.Count () > 10) {
						if (node.Next.Value.ticks < ticks - 5000) {
							components.LastNode ().MoveTo (node.Next.Value.components);
						}
					}

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

		public EiLinkedList<ThreadContainer> threads = new EiLinkedList<ThreadContainer> ();

		#endregion

		#region Core

		public void CloseThreads (bool value = true)
		{
			if (value) {
				var iterator = threads.GetIterator ();
				EiLLNode<ThreadContainer> node;
				while (iterator.Next (out node)) {
					node.Value.IsRunning = false;
				}
			}
		}

		#endregion

		#region Subscribe / Unsubscribe

		public EiLLNode<EiUpdateInterface> Subscribe (EiUpdateInterface component)
		{
			return threads.First ().Subscribe (component);
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

