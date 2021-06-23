using System;
using System.Collections;
using System.Collections.Generic;
using MLAPI.Connection;
using UnityEngine;
using UnityEngine.Serialization;

namespace MLAPI.Interest
{
    public interface IInterestHandler
    {
        public void AddObject(in NetworkObject obj);
        public void RemoveObject(in NetworkObject obj);
        public void QueryFor(in NetworkClient client, HashSet<NetworkObject> results);
    }

    public abstract class InterestObjectStorage : ScriptableObject
    {
        public abstract void QueryFor(in NetworkClient client, HashSet<NetworkObject> results);
        public abstract void AddObject(in NetworkObject obj);
        public abstract void RemoveObject(in NetworkObject obj);
        public abstract void UpdateObject(in NetworkObject obj);
    };

    [CreateAssetMenu(fileName = "InterestNode", menuName = "Interest/Nodes/InterestNode", order = 1)]
    [Serializable]
    public class InterestNode : ScriptableObject, IInterestHandler
    {
        public InterestNode()
        {
            m_ChildNodes = new HashSet<InterestNode>();
        }

        public InterestObjectStorage InterestObjectStorage;

        public virtual void AddObject(in NetworkObject obj)
        {
            InterestObjectStorage?.AddObject(obj);

            foreach (var c in m_ChildNodes)
            {
                c.AddObject(obj);
            }
        }

        public virtual void RemoveObject(in NetworkObject obj)
        {
            InterestObjectStorage?.RemoveObject(obj);

            foreach (var c in m_ChildNodes)
            {
                c.RemoveObject(obj);
            }
        }

        // externally-called object query function.
        //  The passed-in hash set will contain the results.
        public virtual void QueryFor(in NetworkClient client, HashSet<NetworkObject> results)
        {
            InterestObjectStorage?.QueryFor(client, results);

            foreach (var c in m_ChildNodes)
            {
                c.QueryFor(client, results);
            }
        }

        public void UpdateObject(in NetworkObject obj)
        {
            InterestObjectStorage?.UpdateObject(obj);
        }

        // Add a new child node.  Currently, there is no way to remove a node
        public void AddNode(InterestNode newNode)
        {
            m_ChildNodes.Add(newNode);
        }

        protected HashSet<InterestNode> m_ChildNodes;
    }
}
