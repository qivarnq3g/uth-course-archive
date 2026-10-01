#include "DoublyLinkedList.h"
#include <iostream>

int main() {
    DoublyLinkedList dll;

    std::cout << "=== Testing append ===" << std::endl;
    dll.append("hello");
    dll.append("world");
    dll.append("goodbye");
    dll.traverseForward();

    std::cout << "=== Testing prepend ===" << std::endl;
    dll.prepend("first");
    dll.traverseForward();

    std::cout << "=== Testing findByIndex ===" << std::endl;
    Node* n = dll.findByIndex(2);
    if (n) std::cout << "Index 2 = " << n->data << std::endl;

    std::cout << "=== Testing deleteNode('world') ===" << std::endl;
    dll.deleteNode("world");
    dll.traverseForward();

    std::cout << "=== Traverse backward ===" << std::endl;
    dll.traverseBackward();

    return 0;
}
