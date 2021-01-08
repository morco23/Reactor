# Reactor
The reactor is an event handling design pattern that operate in the way of event-driven architecture.
In the reactor, instead of opening a thread for each connection, we use one thread whose function is to monitor the events for the sources, and if it detects an event, it routes it to the handling function.
The Reactor consists of the following:
Synchronous Event Demultiplexer: Monitors all resources and in case of event sends them to the dispatcher.
Dispatcher: Dispatches resources from the demultiplexer to the associated handler. In addition, it is responsible for registering resources and their handlers.
Handler: Function to handle resource in case of event.
Resource: Any resource that can provide input to or consume output from the system.
The benefit of using reactor is minimizing threads that spend time waiting for resource availability, for example, waiting for a resource that can be read from and this way to save system resources. 
