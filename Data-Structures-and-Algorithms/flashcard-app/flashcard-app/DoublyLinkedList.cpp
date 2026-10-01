#include "DoublyLinkedList.h"
#include <iostream>

DoublyLinkedList::DoublyLinkedList() {
    head = nullptr;
    tail = nullptr;
}

DoublyLinkedList::~DoublyLinkedList() {
    Node* current = head;
    while (current) {
        Node* next = current->next;
        delete current;
        current = next;
    }
    head = nullptr;
    tail = nullptr;
}

void DoublyLinkedList::append(std::string value) {
    Node* newNode = new Node(value);

    if (!head) {
        head = tail = newNode;
        return;
    }

    tail->next = newNode;
    newNode->prev = tail;
    tail = newNode;
}

void DoublyLinkedList::prepend(std::string value) {
    Node* newNode = new Node(value);

    if (!head) {
        head = tail = newNode;
        return;
    }

    newNode->next = head;
    head->prev = newNode;
    head = newNode;
}

bool DoublyLinkedList::deleteNode(std::string value) {
    Node* current = head;

    while (current) {
        if (current->data == value) {
            if (current == head) head = current->next;
            if (current == tail) tail = current->prev;

            if (current->prev) current->prev->next = current->next;
            if (current->next) current->next->prev = current->prev;

            delete current;
            return true;
        }
        current = current->next;
    }
    return false;
}

Node* DoublyLinkedList::findByIndex(int index) {
    Node* current = head;
    int i = 0;

    while (current) {
        if (i == index) return current;
        current = current->next;
        i++;
    }
    return nullptr;
}

void DoublyLinkedList::traverseForward() {
    Node* current = head;
    while (current) {
        std::cout << current->data << " ";
        current = current->next;
    }
    std::cout << std::endl;
}

void DoublyLinkedList::traverseBackward() {
    Node* current = tail;
    while (current) {
        std::cout << current->data << " ";
        current = current->prev;
    }
    std::cout << std::endl;
}
