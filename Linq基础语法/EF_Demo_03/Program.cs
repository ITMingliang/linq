using System;

namespace EF_Demo_03
{
    public class Program
    {

        public abstract class Animal { };
        public class Dog:Animal { };

        public interface IMyList<out T>
        {
            T GetElement();  //T如果被in修饰，只能被使用，不能作为返回值
        }

        public class MyList<T> : IMyList<T>
        {
            public T GetElement()
            {
                return default(T);
            }
        }

        public void ChangeT()
        {

        }

        static void Main(string[] args)
        {
           IMyList<Dog> myDogs=new MyList<Dog>();
           IMyList<Animal> myAnimals = myDogs;
        }
    }
}
