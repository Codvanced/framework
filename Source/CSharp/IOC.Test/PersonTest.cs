using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SimpleInjector;
using IOC.FW.Core.Factory;
using IOC.Abstraction.Business;
using IOC.Model;
using IOC.FW.Core.Abstraction.Business;
using IOC.FW.Core.Abstraction.DAO;
using System.Diagnostics;

namespace IOC.Test
{
    [TestFixture]
    public class PersonTest
    {
        public static IEnumerable<Container> Configure()
        {
            yield return InstanceFactory.RegisterModules();
        }

        [Test, TestCaseSource("Configure")]
        public void ObjectTests(Container simpleInjector)
        {
            int hashcode1 = 0;
            int hashcode2 = 0;

            for (int i = 0; i < 50; i++)
            {
                var objBiz = InstanceFactory.GetImplementation<IBaseBusiness<Person>>();
                hashcode1 = objBiz.GetHashCode();
                Debug.WriteLine("Person: BaseBusiness = {0}", hashcode1);

                var objDao = InstanceFactory.GetImplementation<IBaseDAO<Person>>();
                hashcode2 = objDao.GetHashCode();
                Debug.WriteLine("Person: DaoBusiness = {0}", hashcode2);
                
            }
        }

        [Test, TestCaseSource("Configure")]
        public void InsertTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<PersonBusinessAbstract>();
            Assert.NotNull(business);

            business.Insert(new Person
            {
                Gender = "Masculino",
                PersonName = "Pessoa",
                IdOcupation = 1
            });

            var foundPessoa = business.SelectSingle(pessoa =>
                pessoa.Gender == "Masculino"
                && pessoa.PersonName == "Pessoa"
            );

            Assert.NotNull(foundPessoa);
            Assert.Greater(foundPessoa.IdPerson, 0);
        }

        [Test, TestCaseSource("Configure")]
        public void UpdateTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<PersonBusinessAbstract>();
            Assert.NotNull(business);

            var foundPessoa = business.SelectAll();
            Assert.NotNull(foundPessoa);
            Assert.Greater(foundPessoa.Count, 0);

            foundPessoa[0].Gender = "Feminino";

            business.Update(foundPessoa[0]);
            var updatedPessoa = business.SelectSingle(pessoa => pessoa.IdPerson == foundPessoa[0].IdPerson);

            Assert.NotNull(updatedPessoa);
            Assert.AreEqual(updatedPessoa.Gender, foundPessoa[0].Gender);
        }

        [Test, TestCaseSource("Configure")]
        public void DeleteTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<PersonBusinessAbstract>();
            Assert.NotNull(business);

            var foundPessoa = business.SelectAll();
            Assert.NotNull(foundPessoa);
            Assert.Greater(foundPessoa.Count, 0);

            business.Delete(foundPessoa[0]);
            var updatedPessoa = business.SelectSingle(pessoa => pessoa.IdPerson == foundPessoa[0].IdPerson && pessoa.Activated);

            Assert.IsNull(updatedPessoa);
        }

        [Test, TestCaseSource("Configure")]
        public void SelectTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<PersonBusinessAbstract>();
            Assert.NotNull(business);

            var pessoas = business.SelectAll(pessoa => pessoa.Ocupation);
            Assert.NotNull(pessoas);
            Assert.Greater(pessoas.Count, 0);

            var dao = InstanceFactory.GetImplementation<IBaseDAO<Person>>();
            var abc = dao.ExecuteQuery(
                "GetPerson",
                new Dictionary<string, object> 
                { 
                    { "@PAGE",  1 },
                    { "@COUNT", 3 }
                },
                System.Data.CommandType.StoredProcedure
            );

            var testePessoa = business.Select(
                pessoa => pessoa.PersonName == "Teste",
                pessoa => pessoa.Ocupation
            );

            Assert.NotNull(testePessoa);
        }

        [Test, TestCaseSource("Configure")]
        public void ExecTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var business = InstanceFactory.GetImplementation<PersonBusinessAbstract>();
            Assert.NotNull(business);

        }

        [Test, TestCaseSource("Configure")]
        public void ReferenceTest(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var personBusiness = InstanceFactory.GetImplementation<IBaseBusiness<Person>>();
            var ocupationBusiness = InstanceFactory.GetImplementation<IBaseBusiness<Ocupation>>();

            Assert.NotNull(personBusiness);
            Assert.NotNull(ocupationBusiness);

            var ocupation = ocupationBusiness.SelectAll().FirstOrDefault();

            Person person = new Person()
            {
                Gender = "Masculino",
                PersonName = "Pessoa",
                Ocupation = ocupation,
            };

            personBusiness.Insert(person);
        }

        [Test, TestCaseSource("Configure")]
        public void MutipleObjectsWithSameKey(Container simpleInjector)
        {
            Assert.NotNull(simpleInjector);

            var personBusiness = InstanceFactory.GetImplementation<IBaseBusiness<Person>>();
            //var allPersons = personBusiness.SelectAll();
            //var firstPerson = allPersons[0];

            IList<Person> list = new List<Person>() { 
                new Person 
                { 
                    IdPerson = 3,
                    PersonName = "update 1",
                    Gender = "M",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Activated = true,
                    Ocupation = new Ocupation{ IdOcupation = 1 },
                    IdOcupation = 1
                },
                new Person 
                { 
                    IdPerson = 3,
                    PersonName = "update 2",
                    Gender = "F",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Activated = true,
                    Ocupation = new Ocupation{ IdOcupation = 1 },
                    IdOcupation = 1
                },
                new Person 
                { 
                    IdPerson = 3,
                    PersonName = "update 3",
                    Gender = "F",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Activated = true,
                    Ocupation = new Ocupation{ IdOcupation = 1 },
                    IdOcupation = 1
                },
                new Person 
                { 
                    IdPerson = 3,
                    PersonName = "update 4",
                    Gender = "F",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Activated = true,
                    Ocupation = new Ocupation{ IdOcupation = 1 },
                    IdOcupation = 1
                },
                new Person 
                { 
                    IdPerson = 3,
                    PersonName = "update 6",
                    Gender = "F",
                    Created = DateTime.Now,
                    Updated = DateTime.Now,
                    Activated = true,
                    Ocupation = new Ocupation{ IdOcupation = 1 },
                    IdOcupation = 1
                }

            };

            personBusiness.Update(
                list.ToArray()
            );
        }
    }
}
