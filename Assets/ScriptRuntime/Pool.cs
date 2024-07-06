using UnityEngine;
using System.Collections.Generic;
using System;

public class Pool<T> where T : MonoBehaviour {
    Stack<T> stack;
    Func<T> func;
    int count;

    public Pool(int count, Func<T> func) {
        stack = new Stack<T>(count);
        this.func = func;
        for (int i = 0; i < count; i++) {
            stack.Push(func());
        }
    }

    public T Get() {
        if (stack.Count > 0) {
            return stack.Pop();
        } else {
            return func();
        }
    }

    public void Return(T t) {
        stack.Push(t);
    }
    
}