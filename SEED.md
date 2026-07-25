# Consigna proyecto 2026 2º semestre: código provisto <!-- omit in toc -->

<!-- markdownlint-disable-next-line MD033 -->
<img alt="Logo" src="./assets/Runas_and_Dices.png" width="500">

<!-- markdownlint-disable-next-line MD025 -->
# Tabla de contenido <!-- omit in toc -->

<!-- cSpell:ignore Façade façade -->

* [Código provisto](#código-provisto)
  * [Convenciones de código](#convenciones-de-código)
  * [Façade vs Bot](#façade-vs-bot)
  * [Comandos del bot](#comandos-del-bot)
  * [Otras clases](#otras-clases)
  * [Casos de prueba](#casos-de-prueba)

<!-- omit in toc -->
<!-- markdownlint-disable-next-line MD025 -->
# Código provisto

Este repositorio no sólo contiene la consigna del proyecto, sino un código
"semilla" para que hagas crecer tus propias entregas a partir de él.

¿Qué hay en este repositorio?

1. Un proyecto de biblioteca (creado con [`dotnet new classlib --name
   Library`](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new?tabs=netcore22))
   en la carpeta `src/Library`. El código en este proyecto está separado en
   carpetas como `./src/Library/Commands`, `./src/Library/Domain`. Con ese
   código se implementan las [historias de usuario](./STORIES.md) 1, 2, 3 y 4.

2. Un proyecto de aplicación de consola, creado con [`dotnet new console --name
   Program`](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new?tabs=netcore22),
   en la carpeta `src/Program`. En ese proyecto está la implementación del bot
   de Discord en C# utilizando un patrón llamado
   [Façade](https://refactoring.guru/design-patterns/facade).

3. Un proyecto de prueba en [NUnit](https://nunit.org/), creado con [`dotnet new
   nunit --name
   LibraryTests`](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-new?tabs=netcore22),
   en la carpeta `test\LibraryTests`. Ese proyecto de prueba incluye casos de
   prueba para todas las clases provistas.

4. Un proyecto de [Doxygen](https://www.doxygen.nl/index.html) para generación
   de sitio web de documentación en la carpeta `docs`.

5. Análisis estático con [Roslyn
   analyzers](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/overview)
   en los proyectos de biblioteca y de aplicación.

6. Análisis de estilo con
   [StyleCop](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/README.md)
   en los proyectos de biblioteca y de aplicación.

7. Una solución `RunasAndDices.sln` que referencia todos los proyectos de C# y
   facilita la compilación con [`dotnet
   build`](https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-build).

8. Tareas pre-configuradas para ejecutar las pruebas con cobertura y generar
   documentación desde VSCode en la carpeta `.vscode`.

9. Análisis de cobertura de los casos de prueba mediante los indicadores que
   aparecen en los márgenes con el complemento de Visual Studio Code [Coverage
   Gutters](https://marketplace.visualstudio.com/items?itemName=ryanluker.vscode-coverage-gutters).

10. Ejecución automática de compilación y prueba mediante [GitHub
    Actions](https://docs.github.com/en/actions) configuradas en el repositorio
    al hacer [push](https://github.com/git-guides/git-push) o [pull
    request](https://docs.github.com/en/github/collaborating-with-pull-requests).

> [!IMPORTANT]
> Pueden ver que es posible programar toda la funcionalidad pedida en la
> *façade* sin preocuparse por el bot de Discord y agregar al final la
> funcionalidad del bot sin modificaciones a la *façade* en la ultima entrega.

## Convenciones de código

[Convenciones de código en
C#](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions)

[Convenciones de nombres en
C#](https://docs.microsoft.com/en-us/dotnet/standard/design-guidelines/naming-guidelines)

[C# Compiler Errors
(CS*)](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/)

[Roslyn Analyzer Warnings
(CA*)](https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/categories)

[StyleCop Analyzer Warnings
(SA*)](https://github.com/DotNetAnalyzers/StyleCopAnalyzers/blob/master/DOCUMENTATION.md)

Las violaciones a estas convenciones son reportadas como *warnings* al compilar.
Aunque recomendamos corregir las violaciones, es posible omitir esta
configuración de la siguiente forma:

Comentar las siguientes líneas en los archivos de proyecto (`*.csproj`)

```xml
    <EnableNETAnalyzers>true</EnableNETAnalyzers>
    <AnalysisMode>All</AnalysisMode>
    <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
```

Comentar la línea `<PackageReference Include="StyleCop.Analyzers"
Version="1.1.118"/>` en los archivos de proyecto (`*.csproj`)

## Façade vs Bot

Para que vean cómo es posible implementar las historias de usuario usando un
*façade*, este programa puede opcionalmente recibir un parámetro desde la línea
de comando. En caso de que haya un parámetro, se asume que es un nombre de
usuario, y se pasa al método `string GetUserInfo(string )` de la clase
[`Facade`](./src/Library/Domain/Facade.cs) que muestra información de ese
usuario. El bot de Discord no participa en este caso.

Cuando no se pasa un parámetro, se ejecuta el bot de Discord. Cuando un usuario
escribe el comando `!who` en Discord, se termina invocando el método `Task
ExecuteAsync(string)` de la clase
[`UserInfoCommand`](./src/Library/Commands/UserInfoCommand.cs), que a su vez
llama al mismo método `string GetUserInfo(string )` de la fachada.

La carpeta [`Domain`](/src/Library/Domain/) tienen todas las clases utilizadas
por la *façade*.

## Comandos del bot

La forma como están implementados esos comandos es mediante un patrón llamado
[`Command`](https://refactoring.guru/design-patterns/command): cada comando es
un objeto de tipo `CommandBase` que en el método `Task ExecuteAsync(string)`
recibe como parámetro lo que quiera que hayas escrito en el mensaje. Con esta
información, el comando invoca métodos en un objeto `Facade`, que interactuando
con objetos del dominio, implementan las [historias de usuario](./STORIES.md).

La `Facade` implementa otro patrón llamado, justamente,
[Façade](https://refactoring.guru/design-patterns/facade): actúa como una
fachada que oculta un sinnúmero de objetos complejos, como tus objetos del
dominio. De esa forma, los comandos no tienen idea de cuáles son esos objetos ni
cómo se implementan las historias de usuario. Además, y esto es muy importante,
puedes probar el funcionamiento de `Facade` independientemente del bot.

Hay un solo objeto `Facade`, gracias a otro patrón, llamado
[`Singleton`](https://refactoring.guru/design-patterns/singleton). Esto es para
que todos los mensajes sean recibidos por el mismo objeto.

El bot que te damos ya responde a los mensajes que te contamos [aquí](./BOT.md):

* `!who [username]`: Este comando está implementado en la clase
  [`UserInfoCommand`](./src/Library/Commands/UserInfoCommand.cs). No corresponde
  a ninguna historia de usuario.

* `!play [{username}]`: Este comando implementa las [historias de
  usuario](./STORIES.md) números 1 y 3. Está implementado en la clase
  [`PlayCommand`](./src/Library/Commands/PlayCommand.cs).

* `!waitlist`. Este comando implementa la [historia de usuario](./STORIES.md)
  número 2. Está implementado en la clase
  [`WaitListCommand`](./src/Library/Commands/WaitListCommand.cs).

* `!who as:{alias}`, `!play as:{alias}`, y todos los comandos que tu implementes
  como sucesores de [`CommandBase`](./src/Discord/CommandBase.cs): funcionan
  igual que el comando sin incluir `as:{alias}` como si el comando hubiera sido
  enviado por un usuario `alias`. Esto implementa la [historia de
  usuario](./STORIES.md) número 4.

## Otras clases

Además de las clases para los comandos y la *façade*, hay algunos prototipos de
clases del dominio, necesarias para implementar las tres primeras historias de
usuario:

* La clase [`User`](./src/Library/Domain/User.cs) representa un usuario de la
  aplicación. Las instancias de `User` son creadas automáticamente por la
  *façade* a través de la clase
  [`UsersRepository`](./src/Library/Domain/UsersRepository.cs) cuando es
  necesario comenzar a asociar mensajes de un usuario de Discord con acciones de
  un usuario en el juego. Esto ocurre en los métodos `string GetUserInfo(string
  userName)`, `void AddUserToWaitingList(string)`, `Game StartGame(string,
  string)` de la clase [`Facade`](./src/Library/Domain/Facade.cs). En principio,
  no sería necesario modificar la lógica de creación de usuarios en los métodos
  mencionado en este punto. En cambio, es casi seguro que tengas que agregar
  atributos y métodos en la clase `User`, el código provisto es una
  implementación correcta, pero mínima, para que resolver las primeras historias
  de usuario.

* Todas las instancias de `User` son creadas por, y están contenidas en, una
  clase [`UsersRepository`](./src/Library/Domain/UsersRepository.cs). Esta clase
  implementa otro patrón llamado, justamente,
  [`Repository`](https://martinfowler.com/eaaCatalog/repository.html). Como la
  clase `Facade` es la única que usa el repositorio de usuarios, esta clase no
  es un singleton. También vas a tener que modificar la clase `UsersRepository`
  para agregar nuevos métodos que te permitan implementar el resto de las
  historias de usuario.

* La clase `Game` representa una partida entre dos jugadores. Una instancia de
  esta clase es creada durante la historia de usuario #3, que permite iniciar
  una partida entre un jugador y un oponente que está esperando para jugar, en
  el método `string StartGame(string, string)` de la clase
  [`Facade`](./src/Library/Domain/Facade.cs). Esta clase es claramente un
  prototipo mínimo y obviamente no tiene nada implementado en la relación a las
  historias de usuario #4 en adelante.

* Por último, la clase `Player` representa a un jugador para un usuario en el
  juego. Una instancia de esta clase es creada para cada jugador justo antes de
  crear la instancia de `Game` mencionada más arriba, obviamente también en el
  método `string StartGame(string, string)` de la clase
  [`Facade`](./src/Library/Domain/Facade.cs). Esta clase también es un prototipo
  mínimo y tampoco tiene nada implementado en relación a las historias de
  usuario #4 en adelante.

Todas estas clases están en la carpeta [`Domain`](./src/Library/Domain/).

Mira el [diagrama de clases](./DIAGRAM.MD) para ver cómo se relacionan las clases que te
damos; agregarás tus propias clases y las modificaciones que te damos en ese archivo.

## Casos de prueba

Te damos programados los casos de prueba de todas las clases del
[dominio](./test/LibraryTests/Domain) y de todos los
[comandos](./test/LibraryTests/Commands/). No hay casos de prueba para las
clases que dependen de Discord —en [esta carpeta](./src/Discord) que
hemos decidido no probar—.

A medida que lo pidamos en las entregas, deberás agregar casos de prueba para
tus nuevas clases del dominio, para la *façade* y para los comandos.
