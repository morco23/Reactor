
namespace MorCohen
{
    public class ReactorBuilder
    {
        public static Reactor CreateReactor()
        {
            Dispatcher dispatcher = new Dispatcher();
            Demultiplexer demultiplexer = new Demultiplexer(dispatcher);
            return new Reactor(demultiplexer, dispatcher);
        }
    }
}
