#include <iostream>

using namespace std;

class Base{ //Lop cha
    public: int a_public;
    protected: int b_protected;
    private: int c_private;
};
class Derived: private Base{//Con
    public:void display(){//Inside the Derived class
        cout << a_public;
        cout << b_protected;
        cout << c_private;
}};
class Derived2: public Derived{//Chau
    public:void display(){//Inside the Derived class
        cout << a_public;//!!!!!
        cout << b_protected;//!!!!
        cout << c_private;
}};
int main(){    //Outside the class 
    Base b;
    cout << b.a_public;
    cout << b.b_protected;
    cout << b.c_private;
    Derived d;
    cout << d.a_public;//!!!!
    cout << d.b_protected;
    cout << d.c_private;
    return 0;
}

