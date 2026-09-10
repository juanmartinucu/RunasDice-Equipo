# Modelo de dominio - primera entrega

Este es el modelo simplificado que se utilizará para la primera entrega.
Incluye 20 tipos principales, no utiliza herencia entre las clases del juego y
usa una única interfaz, `ICard`, para expresar los datos comunes de las cartas.

`GameState`, `GamePhase`, `DiceType` y `EffectType` son enumeraciones de apoyo;
no se consideran clases del modelo.

## Diagrama de clases

```mermaid
classDiagram
    direction LR

    class User {
        +string UserName
    }

    class Player {
        +User User
        +int Life
        +int Ether
        +Deck Deck
        +Hand Hand
        +Board Board
        +Graveyard Graveyard
        +ReceiveDamage(int amount)
        +Heal(int amount)
        +ResetEther()
        +AddEther(int amount)
        +SpendEther(int amount)
        +DrawCard() ICard
        +Discard(ICard card)
    }

    class Game {
        +Player Player1
        +Player Player2
        +Player ActivePlayer
        +Player DefenderPlayer
        +GameState State
        +GamePhase Phase
        +Player Winner
        +Start()
        +ChangePhase(GamePhase phase)
        +ChangeTurn()
        +Finish(Player winner)
        +GetOpponent(Player player) Player
    }

    class IUsersRepository {
        <<interface>>
        +AllUsers
        +Find(string userName) User
        +Add(string userName) User
    }

    class UsersRepository {
        +AllUsers
        +Find(string userName) User
        +Add(string userName) User
    }

    class Facade {
        <<singleton>>
        +Instance
        +GetUserInfo(string userName)
        +AddUserToWaitingList(string userName)
        +UserIsWaiting(string userName)
        +GetUsersWaitingForOpponent()
        +StartGame(string userName, string opponentName)
        +Reset()
    }

    class Result {
        +bool IsSuccess
        +bool IsFailure
        +string Errors
        +Success()
        +Failure(string error)
    }

    class Result_T_["Result&lt;T&gt;"] {
        +T Value
    }

    class ICard {
        <<interface>>
        +string Name
        +int Cost
        +string Description
    }

    class Creature {
        +string Name
        +int Cost
        +string Description
        +int Attack
        +int Defense
        +bool IsDestroyed
        +List~Upgrade~ Upgrades
        +AddUpgrade(Upgrade upgrade)
        +ModifyStats(int attackChange, int defenseChange)
        +Destroy()
    }

    class Spell {
        +string Name
        +int Cost
        +string Description
        +Effect Effect
    }

    class Upgrade {
        +string Name
        +int Cost
        +string Description
        +Effect Effect
        +bool IsEquipped
        +Equip()
    }

    class Deck {
        +IReadOnlyList~ICard~ Cards
        +Add(ICard card)
        +Draw() ICard
        +Shuffle()
    }

    class Hand {
        +IReadOnlyList~ICard~ Cards
        +Add(ICard card)
        +Remove(ICard card) bool
    }

    class Board {
        +IReadOnlyList~Creature~ Creatures
        +IReadOnlyList~Upgrade~ Upgrades
        +AddCreature(Creature creature)
        +AddUpgrade(Upgrade upgrade)
        +RemoveCreature(Creature creature) bool
    }

    class Graveyard {
        +IReadOnlyList~ICard~ Cards
        +Add(ICard card)
    }

    class Dice {
        +DiceType Type
        +Dice(DiceType type, int[] faces)
        +Roll() DiceResult
    }

    class DiceResult {
        +int Value
    }

    class Effect {
        +EffectType Type
        +int Amount
        +Apply(Player player)
    }

    class Combat {
        +Creature Attacker
        +Creature Defender
        +Resolve(Dice attackDice, Dice defenseDice) bool
    }

    class GameState {
        <<enumeration>>
        Preparing
        InProgress
        Finished
    }

    class GamePhase {
        <<enumeration>>
        Start
        Draw
        Main
        Combat
        End
    }

    class DiceType {
        <<enumeration>>
        StandardNumeric
        Power
        Risk
        Healing
        Rune
    }

    class EffectType {
        <<enumeration>>
        Damage
        Heal
        AddEther
        DrawCard
        ModifyStats
    }

    Facade ..> IUsersRepository : usa
    Facade ..> Game : crea y coordina
    Facade ..> Result : devuelve
    UsersRepository ..> IUsersRepository : implementa
    Player --> User : representa
    Game *-- "2" Player : contiene
    Player *-- Deck
    Player *-- Hand
    Player *-- Board
    Player *-- Graveyard
    Board o-- "0..*" Creature
    Board o-- "0..*" Upgrade
    Creature o-- "0..*" Upgrade : equipa
    Creature ..> ICard : usa
    Spell ..> ICard : usa
    Upgrade ..> ICard : usa
    Deck o-- "0..*" ICard
    Hand o-- "0..*" ICard
    Graveyard o-- "0..*" ICard
    Spell --> Effect
    Upgrade --> Effect
    Effect --> EffectType
    Dice --> DiceType
    Dice --> DiceResult
    Combat --> Creature
    Combat --> Dice
    Game --> GameState
    Game --> GamePhase
```

## Lista de clases y responsabilidades

| Clase | Responsabilidad | Colabora con |
| --- | --- | --- |
| `User` | Identificar al usuario de la aplicación. | `UsersRepository`, `Player` |
| `Player` | Administrar vida, Ether y zonas propias. | `User`, `Game`, `Deck`, `Hand`, `Board`, `Graveyard` |
| `Game` | Controlar jugadores, turnos, fases, estado y ganador. | `Player`, `Combat` |
| `IUsersRepository` | Definir cómo buscar y agregar usuarios. | `Facade`, `UsersRepository` |
| `UsersRepository` | Guardar y buscar usuarios. | `User` |
| `Facade` | Recibir solicitudes externas y coordinar casos de uso. | `UsersRepository`, `Game`, `Result` |
| `Result` | Informar éxito o error de una operación. | `Facade` |
| `Result<T>` | Devolver un resultado junto con un valor. | `Facade` |
| `ICard` | Definir nombre, costo y descripción comunes de una carta. | `Creature`, `Spell`, `Upgrade`, zonas |
| `Creature` | Mantener ataque, defensa, mejoras y estado de destrucción. | `Upgrade`, `Board`, `Combat` |
| `Spell` | Representar una carta que posee un efecto. | `Effect`, `Player` |
| `Upgrade` | Representar una mejora equipable. | `Creature`, `Board`, `Effect` |
| `Deck` | Agregar, barajar y robar cartas. | `Player`, `ICard` |
| `Hand` | Mantener las cartas que el jugador puede jugar. | `Player`, `ICard` |
| `Board` | Mantener criaturas y mejoras activas. | `Player`, `Creature`, `Upgrade` |
| `Graveyard` | Conservar cartas descartadas o destruidas. | `Player`, `ICard` |
| `Dice` | Lanzar un dado según un tipo y sus caras. | `DiceResult`, `Combat` |
| `DiceResult` | Representar el valor obtenido al lanzar un dado. | `Dice`, `Combat` |
| `Effect` | Aplicar daño, curación, Ether u otro efecto básico. | `Player`, `Spell`, `Upgrade` |
| `Combat` | Comparar ataque y defensa y marcar una criatura destruida. | `Creature`, `Dice` |

## Decisiones sobre SRP y Expert

- `Player` modifica su vida y Ether porque es quien posee esos datos.
- `Deck`, `Hand`, `Board` y `Graveyard` administran exclusivamente sus propias
  colecciones.
- `Creature` conoce sus estadísticas y sus mejoras.
- `Combat` coordina una operación que necesita información de dos criaturas y
  de dos dados.
- `Game` controla el estado de la partida, pero no administra internamente las
  cartas ni calcula los dados.
- `Facade` comunica el exterior con el dominio, pero no contiene las reglas del
  combate.
- `ICard` evita repetir el contrato común de las cartas sin crear una clase
  abstracta ni una jerarquía de herencia.

## Nota sobre el código provisto

`Result<T>` ya viene implementado en la semilla heredando de `Result`. Esa
relación pertenece al código entregado por la universidad. En las nuevas
clases del juego no se agrega ninguna jerarquía de herencia: las cartas usan la
interfaz `ICard`.
