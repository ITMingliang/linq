using System;
using System.Collections.Generic;
using System.Linq;

namespace EF_Demo_02
{
    public class Program
    {

        //父类
        public class Animal{  };
        //子类
        public class Dog : Animal { };


        static void Main(string[] args)
        {

            #region 协变与逆变

            //协变
            Dog dog = new Dog();
            Animal animal = dog;

            List<Dog> dogs = new List<Dog>();

            //List<Dog>并不是继承List<Animal>，报错
            //List<Animal> animals = dogs;

            //可以改成如下：
            List<Animal> animals = dogs.Select(d=>(Animal)d).ToList();//遍历强制转换成Animal类型，再转成List

            //out 协变  输出结果，一个泛型参数标记为out,代表是用来输出的，作为结果返回
            //in  逆变  作为参数，代表用来输入的，只能作为参数

            IEnumerable<Dog> someDogs=new List<Dog>();
            IEnumerable<Animal> someAnimal = someDogs;

            Action<Animal> actionAnimal = new Action<Animal>(d => { /*让狗叫*/});
            Action<Dog> actionDog = actionAnimal;
            actionDog(dog);





            #endregion
        }
    }
}
