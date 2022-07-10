// Author: Cemal A. Aydeniz 
// https://github.com/cemalaydeniz
//
// Licensed under the MIT. See LICENSE in the project root for license information


namespace JUtility.JObjectPool
{
    /// <summary>
    /// A helper class for the <see cref="JObjectPool{T}"/><para/>
    /// The data is saved in the <see cref="Data"/> property and the status of the node can be checked by using the <see cref="IsInUse"/> property. Which <see cref="JObjectPool{T}"/> this node is in also can be checked by using
    /// the <see cref="GetPool"/> method.
    /// </summary>
    /// <typeparam name="T">Type of the data</typeparam>
    public class JObjectPoolNode<T>
    {
        internal JObjectPool<T> pool = null!;
        /// <summary>
        /// Gets the <see cref="JObjectPool{T}"/> that this node belongs to.
        /// </summary>
        /// <returns><see cref="JObjectPool{T}"/> that this node belongs to.</returns>
        public JObjectPool<T> GetPool() => pool;

        //~ Begin - Double linked list nodes
        internal JObjectPoolNode<T> next = null!;
        internal JObjectPoolNode<T> previous = null!;
        //~ End


        /// <summary>
        /// The data that is saved inside of this node.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Whether this node is in use or not.
        /// </summary>
        public bool IsInUse { get; internal set; }


        internal JObjectPoolNode(JObjectPool<T> pool)
        {
            this.pool = pool;
            IsInUse = false;

            Data = default;
        }

        internal void OnRemove()
        {
            pool = null!;
            next = null!;
            previous = null!;

            if (default(T) == null)
            {
                Data = default;
            }
        }


        /// <summary>
        /// Returns this node to its related <see cref="JObjectPool{T}"/>
        /// </summary>
        public void ReturnToPool()
        {
            if (pool == null)
                return;

            pool.ReturnNode(this);
        }
    }
}
