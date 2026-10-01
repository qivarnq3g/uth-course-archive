#ifndef NODE_H
#define NODE_H

#include <string>

class Node {
public:
    std::wstring data;
    Node* next;
    Node* prev;

    Node(std::wstring value);
};

#endif

