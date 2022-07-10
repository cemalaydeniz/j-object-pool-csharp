# J Object Pool
This is the Object Pool data structure created with a double linked list.

# How does it work?
The logic behind the J Object Pool is that the head node always shows the next available node while the tail node always shows the last reserved node that is still in use. When a node is needed, the pool returns the head node and put the node at the end of the list, which the node becomes the new tail node. If the head node is not avaialable to use, the pool's size is increased first and then it returns an available node.

# How to use?
The data structure is under the `JUtility.JObjectPool` namespace.
```csharp
using JUtility.JObjectPool;


JObjectPool<MyClass> pool = new JObjectPool<MyClass>(50, 10);   // The initial size of the pool is 50 and it will increase by 10 each time there is no available node in the pool.


// Get the next available node by simply calling the GetNode method.
JObjectPoolNode<MyClass> node = pool.GetNode();

// If the data inside of the node is a nullable type, the Data property must be checked if it is null for first use.
if (node.Data == null)
{
    node.Data = new MyClass();
}
node.Data...   // Use the properties/values/methods inside of the Data property.


// When the node is not needed anymore, it must be returned to the pool.
node.ReturnToPool();
```

# Big O Notations
`JObjectPool`
| Function Name          | Notation |
| ---------------------- | ---------|
| Constructor            | O(n)     |
| Deconstructor          | O(n)     |
| IncreasePoolSize       | O(n)     |
| GetNode                | O(1)     |
| GetAllNodes            | O(n)     |
| ReturnNode             | O(1)     |
| RemoveNode             | O(1)     |
| ClearPool              | O(n)     |
| GetCount               | O(1)     |
| GetIncrementSize       | O(1)     |
| SetIncrementSize       | O(1)     |
| GetNumofNodesInUse     | O(1)     |
| GetNumofAvailableNodes | O(1)     |

`JObjectPoolNode`
| Function Name    | Notation |
| ---------------- | ---------|
| Constructor      | O(1)     |
| Deconstructor    | O(1)     |
| ReturnToPool     | O(1)     |
| GetData          | O(1)     |
| SetData          | O(1)     |
| SetDataDeleteOld | O(1)     |
| GetPool          | O(1)     |
| IsInUse          | O(1)     |

# License
This project is licensed under the MIT License - see the LICENSE.md file for details.