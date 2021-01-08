using System;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MorCohen
{
    public class AsyncResourceHandler<T> : IResourceHandler
    {
        private Func<Socket, T> m_dataRecv;
        private Action<T, Socket> m_dataWorker;
        private Socket m_socket;

        /// <summary>
        /// An async resource handler.
        /// Note: Operation on socket and reactor should only used through  sync handler becouse it can lead to unexpected behaviour due the way reactor performs.
        /// </summary>
        /// <param name="dataRecv">Sync handler to operate on the socket and reactor. </param>
        /// <param name="dataWorker">Async handler that get the result from the sync handler.</param>
        /// <param name="socket"></param>
        public AsyncResourceHandler(Func<Socket, T> dataRecv, Action<T, Socket> dataWorker = null, Socket socket = null)
        {
            m_dataRecv = dataRecv;
            m_dataWorker = dataWorker;
            m_socket = socket;
        }

        public void Call()
        {
            T ret = default;

            if (m_dataRecv != null)
            {
                ret = m_dataRecv(m_socket);
            }

            if (m_dataWorker != null)
            {
                Task.Run(() =>
                {
                    m_dataWorker(ret, m_socket);
                });

            }
        }
    }
}
