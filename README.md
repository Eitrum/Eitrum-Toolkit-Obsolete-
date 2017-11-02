# EiComponent
A framework to develop and make games with good performance and multi-threaded system for Unity.

How to use!
Everything that is supposed to be attached on game objects should inherit from 'EiComponent'
Everything that is not a component but still want to use system of updates and messages should inherit from 'EiCore'

To subscribe on update loops, write SubscribeUpdate() in awake or constructor to make it subscribe on that specific update loop.
Loops that exists currently are:
* PreUpdate
* Update
* LateUpdate
* FixedUpdate
* ThreadedUpdate

Pre Update and Threaded Update is only new things from Unity, where Pre Update is just called before normal Update and Threaded Update is subscribing on one of n number of threads depending on processor count.


Utility classes

EiTimer - Simple Coroutine wrapper to make timers that is called Once, on repeat, or as lerp over time, aka Animation
EiTask - Simple way of doing multi threaded tasks with both callbacks for Non-Unity thread and Unity thread. 
    EiTask.Run(MyFunction, MyLoggingMethod, MyUnityThreadCallback); one small example on how to use it.
    
EiMessage - everything can subscribe to this class with different messages and everyone can Publish messages so all subscribed get a callback. 
  * EiMessage.Publish(new MyMessageClass("Hello World")); 
  * EiMessage.Subscribe<MyMessageClass>(MyListener);
  
EiBuffer - Serializer to save and load data as well send packets for servers.
{
  EiBuffer buffer = new EiBuffer(sizeOfBuffer);
  buffer.SetDynamic(); <- Makes it dynamic and scale as a list, but becomes slower and creates GC on new sizes.
  
  buffer.Write(myInt);
  buffer.Write(myTexture2D);
  buffer.Write(myPositionVec3);
  
  buffer.WriteToFile(pathToFile);
}

EiPropertyEvent<T> - A simple way of doing a property where you can subscribe to on changes. Supports multi thread with callbacks on Unity Thread. 
{
  EiPropertyEvent<float> health = new EiPropertyEvent<float>(100f);
  health.Subscribe(OnHealthChanged);

  health.Value = 74f;
}

EiSyncronizedList + EiSyncronizedQueue - Two wrappers for having threading support with lists and queues.


Both EiComponent and EiCore also has a Singleton structure to be able to create singleton easier.
public class MyComponent : EiComponentSingleton<MyComponent>{}
  This will make a singleton out of this script, it will search in resource folder for a prefab that shares the same name as the script.
  If it doesnt find any, it will create a new game object.
