using System;

namespace EF_Demo_04
{
    public class Program
    {

        public abstract class Animal { };
        public class Dog : Animal { };

        public interface IMyList<in T>
        {
            void GetElement();  //T如果被in修饰，只能被使用，不能作为返回值
        }

        public class MyList<T> : IMyList<T>
        {
            public void GetElement()
            {
               
            }
        }

        static void Main(string[] args)
        {
            IMyList<Animal> myAnimals_B = new MyList<Animal>();
            IMyList<Dog> myDogs_B = myAnimals_B;
        }
    }
}
