using System;
using System.Threading;
using System.Diagnostics;
using Eitrum.Engine.Core.Singleton;
using Eitrum.Engine.Core;

namespace Eitrum.Engine.Threading
{
	public class ThreadedUpdateSystem : ClassSingleton<ThreadedUpdateSystem>
	{
        #region Singleton Creation

        protected override void OnSingletonCreated() {
			var us = UpdateSystem.Instance;
			var processors = Math.Max (1, Environment.ProcessorCount - 1);
			for (int i = 0; i < processors; i++) {
				var thread = new ThreadContainer (10000);
				var node = threads.Add (thread);
				thread.SetNode (node);
			}
			var inst = UnityThreading.Instance;
			UnityThreading.CloseThreads.Subscribe (CloseThreads, true);
		}

		#endregion

		#region Classes

		public class ThreadContainer
		{
			#region Variables

			EiLLNode<ThreadContainer> node;
			Thread thread;
			EiLinkedList<IThreadedUpdate> components = new EiLinkedList<IThreadedUpdate> ();

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
						EiLLNode<IThreadedUpdate> component;
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
								UnityEngine.Debug.LogException (e);
							}
						}
					} catch (Exception e) {
						UnityEngine.Debug.LogException (e);
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

			public EiLLNode<IThreadedUpdate> Subscribe (IThreadedUpdate component)
			{
				return components.Add (component);
			}

			public void Unsubscribe (EiLLNode<IThreadedUpdate> node)
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

		public EiLLNode<IThreadedUpdate> Subscribe (IThreadedUpdate component)
		{
			threads.ShiftNext ();
			return threads.First ().Subscribe (component);
		}

		public void Unsubscribe (EiLLNode<IThreadedUpdate> node)
		{
			if (node.List != null)
				node.List.Remove (node);
			else
				UnityEngine.Debug.LogError ("Cant remove node from system");
		}

		#endregion
	}
}

