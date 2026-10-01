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

    void append(std::wstring value);
    void prepend(std::wstring value);
    bool insertAt(int index, std::wstring value);
    bool deleteNode(std::wstring value);
    bool deleteAt(int index);
    Node* findByIndex(int index);
    Node* search(std::wstring value);
    void traverseForward();
    void traverseBackward();
    
    int traverseForwardCount();
    int traverseBackwardCount();
    
    int getSize();
    Node* getCurrent();
    void setCurrentIndex(int index);
    int getCurrentIndex();
    std::wstring getAllCardsAsString();
    std::wstring getAllCardsAsStringReverse();
    
    bool updateCurrent(const std::wstring& newValue);
    bool deleteCurrent();
    
private:
    int currentIndex;
    Node* currentNode;
};

#endif

