#ifndef NODE_H
#define NODE_H

#include <string>

class Node {
public:
    std::string data;
    Node* next;
    Node* prev;

    Node(std::string value);
};

#endif
