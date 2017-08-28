#Codvanced Framework (a.k.a IOC FW)
**Simplesmente um framework baseado em inversão de controle.**

[![Build status](https://ci.appveyor.com/api/projects/status/b2ojfywi3i2rgqki/branch/master?svg=true)](https://ci.appveyor.com/project/Codvanced/framework/branch/master)

A ideia é agilizar ainda mais o processo de concepção e estruturação de um projeto mantendo os princípíos que afirmam a orientação a objetos descritos como [SOLID](http://en.wikipedia.org/wiki/SOLID_%28object-oriented_design%29).

Para isso desconstruímos alguns raciocínios antiquados, como deixar o desenvolvedor preso a um fluxo de programação contínuo onde ele acaba se vendo preso e engessado, e optamos por um estilo mais flexível e maleável.

##Ferramental escolhido

Para tornar possível tudo isto, optamos por utilizar um ORM de mercado, um container para IoC, um validador com interface fluente, um logger flexível e muito bom senso!

1.  [Entity Framework 6](https://www.nuget.org/packages/EntityFramework)
  * Code first, criando a base na mão e com lazy loading desligado
2.  [SimpleInjector](https://www.nuget.org/packages/SimpleInjector/)
3.  [FluentValidation](https://www.nuget.org/packages/FluentValidation/)
4.  [log4net](https://www.nuget.org/packages/log4net/)


##Seja um programador mais feliz e comece a usar

Chega de papo e vamos ao que interessa de verdade, faça um clone deste repositório e veja na raiz uma pasta chamada: 
* Release
  * Scaffold
    * **IOC.Scaffold.exe** (Este executável criará um projeto novo, configurado e pronto para programar)
    * TEMPLATE_MVC_4
  * FW-References
  * Help Documentation

Para criar um projeto, use o seguinte comando:

>create-project C:\desenvolvimento\projeto\local_do_projeto nome_do_projeto nome_do_cliente 


Agora que você já tem consigo o projeto na pasta escolhida, vamos nos familiarizar com a arquitetura...
Para melhor entendimento, pense proximo ao design pattern de 3 camadas.

Neste momento você verá divisões de pastas para cada "camada" do projeto, existirão três grandes chaves que são estas:

1.  Project Configuration
  * conterá todas as configurações de bindings (abstração para classe concreta) para o Container usado fazer a injeção de dependências
  * No projeto de bindings, terão duas estruturas de injeção, business module e dao module. Para incluir no processo de resolução e criação de objetos, basta incluir a seguinte linha nos arquivos especificos:

  ```cs
    public void SetBinding(Container container)
    {
      container.Register<AbstractPersonBusiness, PersonBusiness>(Lifestyle.Singleton);
    }
  ```
2.  Project Core
  * conterá as models, abstrações e implementações de Business e DAOs 
  * para criar uma abstração nova, siga este exemplo:
  
  Para business:
  ```cs
  public abstract class AbstractPersonBusiness : BaseBusiness<Person>
  {
      public AbstractPersonBusiness(AbstractPersonDAO dao): base(dao)
      { }
  }
  ```
  
  Para dao:
  ```cs
  public abstract class AbstractPersonDAO : BaseRepository<Person>
  {
      public AbstractPersonDAO(string connectionString = null) : base()
      { }
  }
  ```
  
  ###OBS ULTRA IMPORTANTE 1:
  Se o que você precisa já se encontra nas business padrões, não precisará criar abstração alguma, basta incluir sua model como Generic Type da BaseBusiness e pronto!
  
  Ex:
  ```cs
  using SimpleInjector;
  using IOC.Model;
  using IOC.Validation;
  using IOC.Abstraction.Business;
  using IOC.FW.Core.Abstraction.Business;
  using IOC.FW.Core.Factory;
  using IOC.FW.Web.MVC.Base;
  namespace IOC.Web.Controllers
  {
      public class GenreController
          : Controller
      {
          private readonly IBaseBusiness<Genre> _business;
  
          public GenreController(IBaseBusiness<Genre> business)
          {
              this._business = business;
          }
  
          public ActionResult Index()
          {
              var items = _business.SelectAll();
  
              return View(items);
          }
  
          public JsonResult GetJson()
          {
              var items = _business.SelectAll();
              return Json(items, JsonRequestBehavior.AllowGet);
          }
      }
  }
```
  
  ###OBS ULTRA IMPORTANTE 2:
  A BaseBusiness e BaseRepository tem todos os métodos padrões para um CRUD genérico, por isso incluímos estas extensões de classes nas abstrações. Isso não é necessário, é apenas um facilitador, para que você possa criar suas próprias regras customizadas sem ser obrigado a parar de utilizar a camada de ORM escolhido.
  
  
3.  UI
  * conterá todas as camadas visuais (como padrão, aqui já existirá uma MVC 4)
  * Para aplicações MVC 3, 4 ou posterior, já contamos com um resolvedor de dependencias nativo que podemos plugar com o SimpleInjector e obter injeções diretamente via Constructor. Para utilizá-lo, veja como foi feito no global.asax.cs nas três ultimas linhas contidas no Application_Start()
  * Isto permitirá você receber seus objetos mapeados no Configuration, diretamente via Constructor!
  * Se sua aplicação não for Web ou não somente Web, não tem problema, temos uma classe que poderá também devolver depencências usando o mesmo ecossistema configurado.
  
* Exemplo MVC 4:
```cs
  protected void Application_Start()
  {
      AreaRegistration.RegisterAllAreas();
      WebApiConfig.Register(GlobalConfiguration.Configuration);
      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);

      var container = InstanceFactory.RegisterModules(Register.RegisterWebApi);
      var resolve = new SimpleInjectorDependencyResolver(container);
      DependencyResolver.SetResolver(resolve);
  }
```
* Exemplo aplicação não Web:
```cs
  var container = InstanceFactory.RegisterModules();
  var business = InstanceFactory.GetImplementation<IBaseBusiness<Person>>();  
```
  
##Conclusão

Com isso tudo, estamos conseguindo criar projetos com mais qualidade, agilidade e testabilidade já que cada camada pode ser injetada independentemente.

É isso, good coding!

EOF
