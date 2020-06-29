using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tom.Lib.Clusters
{
    public enum NodeState
    {
        UnStarted = 0,
        Runnig = 2,
        Paused = 4,
        Stopped = 0x10,
    }

    public interface INode
    {
        NodeState NodeState { get; }
        bool IsStopped { get; }

        void Start(ThreadStart start);
        void Stop();
        void Pause();
        void Resume();
    }

    public class Node : INode
    {
        readonly Dictionary<NodeState, List<NodeState>> nodeStateMachine;

        public NodeState NodeState { get; private set; }

        public bool IsStopped => NodeState == NodeState.Stopped;

        public Node()
        {
            nodeStateMachine = new Dictionary<NodeState, List<NodeState>>
            {
                { NodeState.UnStarted, new List<NodeState>{ NodeState.Runnig } },
                { NodeState.Runnig, new List<NodeState>{ NodeState.Stopped, NodeState.Paused } },
                { NodeState.Paused, new List<NodeState>{ NodeState.Runnig } },
            };
        }


        public void Resume()
        {
            ChangeState(NodeState.Runnig);
        }

        public void Start(ThreadStart start)
        {
            if (ChangeState(NodeState.Runnig))
            {
                start();
            }
        }

        public void Stop()
        {
            ChangeState(NodeState.Stopped);
        }

        public void Pause()
        {
            ChangeState(NodeState.Paused);
        }

        private bool ChangeState(NodeState to)
        {
            if (nodeStateMachine.ContainsKey(NodeState) && nodeStateMachine[NodeState].Contains(to))
            {
                NodeState = to;
                return true;
            }
            return false;
        }
    }
}
