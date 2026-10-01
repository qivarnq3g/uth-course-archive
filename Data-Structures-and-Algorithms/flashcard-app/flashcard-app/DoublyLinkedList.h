#ifndef DOUBLY_LINKED_LIST_H
#define DOUBLY_LINKED_LIST_H

#include "Node.h"

class DoublyLinkedList {
private:
    Node* head;
    Node* tail;

public:
    DoublyLinkedList();
    ~DoublyLinkedList();

    void append(std::string value);
    void prepend(std::string value);
    bool deleteNode(std::string value);
    Node* findByIndex(int index);
    void traverseForward();
    void traverseBackward();
};

#endif
