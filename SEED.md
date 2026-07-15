# Consigna proyecto 2026 2º semestre: código provisto <!-- omit in toc -->

<!-- markdownlint-disable-next-line MD033 -->
<img alt="Logo" src="./assets/Runas_and_Dices.png" width="500">

<!-- markdownlint-disable-next-line MD025 -->
# Tabla de contenido <!-- omit in toc -->

<!-- cSpell:ignore Façade façade -->

* [Código provisto](#código-provisto)
  * [Façade vs Bot](#façade-vs-bot)
  * [Comandos del bot](#comandos-del-bot)
  * [Otras clases](#otras-clases)
  * [Casos de prueba](#casos-de-prueba)

<!-- omit in toc -->
<!-- markdownlint-disable-next-line MD025 -->
# Código provisto

Este repositorio no sólo contiene la consigna del proyecto, sino un código
"semilla" para que hagas crecer tus propias entregas a partir de él.

El código provisto:

* Implementa las [historias de usuario](./STORIES.md) 1, 2, 3 y 4.

* Muestra cómo implementar un bot de Discord en C# utilizando un patrón llamado
  [Façade](https://refactoring.guru/design-patterns/facade).

> [!IMPORTANT]
> Pueden ver que es posible programar toda la funcionalidad pedida en la
> *façade* sin preocuparse por el bot de Discord y agregar al final la
> funcionalidad del bot sin modificaciones a la *façade* en la ultima entrega.

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
