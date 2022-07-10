// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


namespace JUtility.JObjectPool
{
    /// <summary>
    /// This is the object pool data structure implementation with a double linked list.<para/>
    /// Use it when the cost of initializing a class instance is very high. This is a container that contains some amount of instances. When a class instance is needed, this data structure will return an available object from the pool
    /// with a cost of O(n) = 1. In order to get an instance from the pool, the <see cref="GetNode"/> method should be used. Afterwards, the <see cref="JObjectPoolNode{T}.Data"/> property can be used to access the data inside of the node.
    /// When the node's usage is done, the node must be returned to the pool by using the <see cref="JObjectPoolNode{T}.ReturnToPool"/> method or by using the <see cref="ReturnNode(JObjectPoolNode{T})"/> method.<para/>
    /// In order to have a better performance, the initial size of the pool and the increment size should be adjusted well.
    /// </summary>
    /// <typeparam name="T">Type of the data inside of the pool's nodes</typeparam>
    public class JObjectPool<T>
    {
        //~ Begin - Double linked list nodes
        private JObjectPoolNode<T> head;
        private JObjectPoolNode<T> tail;
        //~ End

        /// <summary>
        /// The number of nodes inside of this pool.
        /// </summary>
        public int Count { get; private set; }

        private int incrementSize;
        /// <summary>
        /// The increment size of the pool. When there is no available node in the pool, the pool creates a new set of nodes depending on this value.
        /// </summary>
        public int IncrementSize
        {
            get => incrementSize;
            set
            {
                incrementSize = value < 0 ? 1 : value;
            }
        }

        /// <summary>
        /// The total number of nodes that are in use.
        /// </summary>
        public int NumofNodesInUse { get; private set; }
        /// <summary>
        /// The total number of available nodes to use.
        /// </summary>
        public int NumofAvailableNodes { get => Count - NumofNodesInUse; }


        /// <summary>
        /// Initializes the pool.
        /// </summary>
        /// <param name="initialSize">The number of the nodes in this pool when it is initialized.</param>
        /// <param name="incrementSize">The number of nodes to create when there is no avaiable node in the pool.</param>
        public JObjectPool(int initialSize, int incrementSize)
        {
            head = null!;
            tail = null!;

            Count = 0;
            IncrementSize = incrementSize;

            IncreasePoolSize(initialSize < 1 ? 5 : initialSize);
        }

        /// <summary>
        /// Adds a new set of nodes to the pool.
        /// </summary>
        /// <param name="incrementSize">The number of nodes to add.</param>
        public void IncreasePoolSize(int incrementSize)
        {
            for (int i = 0; i < incrementSize; i++)
            {
                JObjectPoolNode<T> node = new JObjectPoolNode<T>(this);
                if (head != null && tail != null)
                {
                    node.next = head;
                    head.previous = node;

                    head = node;
                }
                else
                {
                    head = node;
                    tail = node;
                }

                Count++;
            }
        }

        /// <summary>
        /// Gets the next available node in the pool. If there is no available node in the pool, the pool creates a new set of nodes
        /// depending on the <see cref="IncrementSize"/> property.
        /// </summary>
        /// <returns>Returns the next avaiable node.</returns>
        public JObjectPoolNode<T> GetNode()
        {
            if (head.IsInUse)
            {
                IncreasePoolSize(incrementSize);
            }

            JObjectPoolNode<T> node = head;
            head.IsInUse = true;
            NumofNodesInUse++;

            head = head.next;

            head.previous = null!;
            node.next = null!;

            node.previous = tail;
            tail.next = node;
            tail = node;

            return node;
        }

        /// <summary>
        /// Returns the all nodes in the pool. The order of the list is from the available ones to used ones.
        /// </summary>
        /// <returns>Returns the list of the all nodes in the pool.</returns>
        public List<JObjectPoolNode<T>> GetAllNodes()
        {
            List<JObjectPoolNode<T>> nodes = new List<JObjectPoolNode<T>>();

            JObjectPoolNode<T> node = head;
            while (node != null)
            {
                nodes.Add(node);
                node = node.next;
            }

            return nodes;
        }

        /// <summary>
        /// Returns the node to the pool. Use it when the node is no longer needed.
        /// </summary>
        /// <param name="node">The node to return to this pool.</param>
        /// <returns>Returns true if the node is successfully returned to this pool. Returns false if the node does not
        /// belong to this pool.</returns>
        public bool ReturnNode(JObjectPoolNode<T> node)
        {
            if (node.pool != this) return false;
            if (!node.IsInUse) return true;

            if (node != head)
            {
                if (node == tail)
                {
                    tail = node.previous;
                    tail.next = null!;

                    node.previous = null!;
                    node.next = head;

                    head.previous = node;
                    head = node;
                }
                else
                {
                    node.previous.next = node.next;
                    node.next.previous = node.previous;

                    node.previous = null!;
                    node.next = head;

                    head.previous = node;
                    head = node;
                }
            }

            node.IsInUse = false;
            NumofNodesInUse--;

            return true;
        }

        /// <summary>
        /// Removes a node from this pool.
        /// </summary>
        /// <param name="node">The node to remove from this pool.</param>
        /// <returns>Return true if the node is successfully removed from the pool. Returns false if the node
        /// does not belong to this pool.</returns>
        public bool RemoveNode(JObjectPoolNode<T> node)
        {
            if (node.pool != this)
                return false;

            if (Count != 1)
            {
                if (node == head)
                {
                    head = head.next;
                    head.previous = null!;
                }
                else if (node == tail)
                {
                    tail = tail.previous;
                    tail.next = null!;
                }
                else
                {
                    node.previous.next = node.next;
                    node.next.previous = node.previous;
                }
            }
            else
            {
                head = null!;
                tail = null!;
            }

            node.OnRemove();

            Count--;

            return true;
        }

        /// <summary>
        /// Removes all the nodes from the pool.
        /// </summary>
        public void ClearPool()
        {
            JObjectPoolNode<T> node = head;
            while (node != null)
            {
                JObjectPoolNode<T> temp = node;
                node = node.next;
                temp.OnRemove();
            }

            Count = 0;
        }
    }
}
