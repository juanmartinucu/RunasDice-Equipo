# Consigna proyecto 2026 2º semestre: juego `Runas & Dices` <!-- omit in toc -->

<!-- markdownlint-disable-next-line MD033 -->
<img alt="Logo" src="./assets/Runas_and_Dices.png" width="500">

<!-- markdownlint-disable-next-line MD025 -->
# Tabla de contenido <!-- omit in toc -->

* [Juego `Runas & Dices`](#juego-runas--dices)
  * [1. Visión general](#1-visión-general)
  * [2. Objetivo del juego](#2-objetivo-del-juego)
  * [3. Elementos del juego](#3-elementos-del-juego)
    * [3.1 Jugadores y sus zonas](#31-jugadores-y-sus-zonas)
    * [3.2 Tipos de `Card`](#32-tipos-de-card)
    * [3.3 `Dice`: dados especiales](#33-dice-dados-especiales)
  * [4. Estructura de la partida](#4-estructura-de-la-partida)
    * [4.1 `Game`](#41-game)
    * [4.2 Preparación inicial](#42-preparación-inicial)
    * [4.3 Turno y fases](#43-turno-y-fases)
  * [5. Recursos y `Cost` de `Card`](#5-recursos-y-cost-de-card)
  * [6. Reglas de combate y uso de `Dice`](#6-reglas-de-combate-y-uso-de-dice)
    * [6.1 Estadísticas de `Creature`](#61-estadísticas-de-creature)
    * [6.2 Declaración de ataque y resolución de combate](#62-declaración-de-ataque-y-resolución-de-combate)
    * [6.3 Cálculo de daño con `Dice`](#63-cálculo-de-daño-con-dice)
  * [7. `Spell`, `Upgrade` y efectos](#7-spell-upgrade-y-efectos)
  * [8. Dados especiales en `Spell` y `Abilities`](#8-dados-especiales-en-spell-y-abilities)
  * [9. Fin de la `Game`](#9-fin-de-la-game)
  * [10. Ejemplo de set inicial de `Card`](#10-ejemplo-de-set-inicial-de-card)
    * [10.1 `Creature`](#101-creature)
      * [`Apprentice Warrior`](#apprentice-warrior)
      * [`Rune Soldier`](#rune-soldier)
      * [`Spark Mage`](#spark-mage)
      * [`Unstable Beast`](#unstable-beast)
      * [`Stone Giant`](#stone-giant)
    * [10.2 `Spell`](#102-spell)
      * [`Unstable Bolt`](#unstable-bolt)
      * [`Minor Heal`](#minor-heal)
      * [`Ether Surge`](#ether-surge)
      * [`Targeted Destruction`](#targeted-destruction)
      * [`Rune Invocation`](#rune-invocation)
    * [10.3 `Upgrade`](#103-upgrade)
      * [`Fortune Blade`](#fortune-blade)
      * [`Rune Shield`](#rune-shield)
      * [`Protection Aura`](#protection-aura)

<!-- omit in toc -->
<!-- markdownlint-disable-next-line MD025 -->
# Juego `Runas & Dices`

## 1. Visión general

`Runas & Dices` es un juego de cartas y dados especiales por turnos, para dos
jugadores, pensado para jugarse a través de un chat bot.

Cada jugador controla a un mago que invoca criaturas, lanza hechizos y utiliza
mejoras, combinando sus cartas con diferentes tipos de dados para intentar
reducir a 0 la vida del oponente.

Esta especificación describe las reglas del juego y los elementos del dominio.
No describe cómo debe implementarse el código ni cómo se integra con el bot;
eso forma parte del trabajo del proyecto, que iremos publicando a medida que
avanza el curso.

## 2. Objetivo del juego

Cada jugador comienza con 20 puntos de vida. El objetivo es reducir la vida del
oponente a 0 o menos usando criaturas, hechizos y efectos generados por los
dados.

En el momento en que la vida de un jugador es 0 o menos, la partida termina y el
otro jugador es declarado ganador.

## 3. Elementos del juego

A continuación te contamos cuáles son los elementos del juego. A esto le
llamaremos dominio. Una de tus actividades durante el proyecto será crear un
modelo de clases para representar este dominio. A ese modelo de clases le
llamaremos modelo de dominio.

### 3.1 Jugadores y sus zonas

En el contexto de una partida o `Game`, cada jugador se representa como un
`Player` que posee:

* `Life`: cantidad actual de vida, es un número entero, por defecto 20.
* `Deck`: conjunto ordenado de `Card` boca abajo, desde donde roba.
* `Hand`: conjunto de `Card` que el jugador tiene en la mano y puede jugar.
* `Board`: conjunto de `Card` que están actualmente en juego, pueden ser
  `Creature` y algunas `Upgrade`.
* `Graveyard`: conjunto de `Card` usadas o destruidas, que ya no están activas.
* `Ether`: recurso que se usa para pagar el costo de las `Card`.

Cada `Player` pertenece a una `Game` concreta.

### 3.2 Tipos de `Card`

En el juego existen distintos tipos de `Card`. A nivel de reglas, toda `Card`
tiene como mínimo:

* `Name`: nombre de la `Card`.
* `Cost`: costo para jugarla, en términos de `Ether`.
* `Description`: texto descriptivo de su efecto.

Sobre esta base, se definen tres grandes categorías:

1. Cartas `Creature`

   * Representa una criatura que permanece en el `Board`.
   * Atributos adicionales típicos:
     * `AttackBase`: ataque base.
     * `DefenseBase`: defensa base, o “vida” de la criatura.
   * Puede tener `Abilities` especiales.
   * Puede declarar ataques durante la `Combat Phase`.

2. Cartas `Spell`

   * Representa un efecto puntual que se resuelve al jugar la `Card`.
   * Después de resolver su efecto, la `Spell` va al `Graveyard`.
   * Ejemplos: daño directo, curación, robo de `Card`, modificación de stats, etc.

3. Cartas `Upgrade`

   * Representa una mejora que se equipa a una `Creature` o a un `Player`.
   * Permanece en el `Board` mientras no sea destruida.
   * Modifica atributos o agrega `Abilities`, por ejemplo: `+Attack` a una
      `Creature`, reducción de daño al `Player`, interacción especial con
      `Dice`, etc.

### 3.3 `Dice`: dados especiales

En `Runas & Dices` no hay un único dado clásico de seis caras. En su lugar,
existe un concepto general de `Dice`, con varias variantes:

* Un `Dice` es un dado que, al ser lanzado, produce un `DiceResult`.
* Un `DiceResult` puede ser:
  * Un valor numérico.
  * Un símbolo, por ejemplo, una runa.
  * O una estructura más compleja que luego las `Card` interpretan.

Ejemplos de tipos de `Dice`:

1. `StandardNumericDice`

   * Caras: 1, 2, 3, 4, 5, 6.
   * Se usa para efectos neutros y combate básico.

2. `PowerDice`

   * Diseñado para favorecer resultados altos.
   * Caras: 4, 5, 6, 4, 5, 6
   * Se usa para ataques especialmente potentes.

3. `RiskDice`

   * Caras muy extremas, por ejemplo: 0, 0, 3, 4, 7, 8.
   * Representa efectos muy poderosos pero impredecibles: puede no hacer nada o
     producir mucho daño.

4. `HealingDice`

   * Sus caras están orientadas a la curación o protección.
   * Ejemplo de caras posibles:
     * “heal 1 `Life`”
     * “heal 2 `Life`”
     * “heal 3 `Life`”
     * “heal 1 `Life` and draw 1 `Card`”
     * “prevent 1 damage this turn”
     * “heal 4 `Life`”

5. `RuneDice`

   * Sus caras no son números, sino símbolos: `Fire`, `Water`, `Earth`, `Air`,
     `Light`, `Shadow`.
   * Algunas `Card` interpretan esos símbolos:
     * `Fire`: daño.
     * `Water`: curación.
     * `Earth`: aumento de defensa.
     * `Air`: robo de `Card`.
     * `Light`: efectos positivos al `Player`.
     * `Shadow`: efectos que dañan a ambos `Player`, etc.

A nivel de reglas, cuando una `Card` dice “roll a `PowerDice`” o “roll a
`RuneDice`”, se lanza el `Dice` correspondiente y se aplica el efecto de acuerdo
al `DiceResult`.

## 4. Estructura de la partida

### 4.1 `Game`

Un `Game` representa una partida entre dos `Player`. Contiene, entre otros
datos:

* Los dos `Player` participantes.
* El `Player` cuyo turno está activo, `ActivePlayer`.
* El `Player` oponente en ese turno, `DefenderPlayer`.
* El estado general de la `Game`: en preparación, en curso, finalizada.
* La fase actual del turno, `GamePhase`.

### 4.2 Preparación inicial

Antes de empezar la `Game`:

1. Cada `Player` define su `Deck`, con un tamaño prefijado; por defecto, 30
   `Card`.
2. Cada `Player` baraja su `Deck`.
3. Cada `Player` roba una `StartingHand`; por defecto, 5 `Card`.
4. La `Life` inicial de cada `Player` se establece en un valor inicial, por
   defecto 20.
5. Se determina de forma aleatoria quién será el primer `ActivePlayer`.

### 4.3 Turno y fases

La `Game` se desarrolla en turnos alternados entre los dos `Player`. Cada turno
se divide en las siguientes fases:

1. `Start Phase`

   * Se resuelven efectos que digan “al inicio de tu turno”.
   * Se limpian estados temporales que caducan “hasta el fin del turno anterior”.

2. `Draw Phase`

   * El `ActivePlayer` roba una `Card` de su `Deck` y la añade a su `Hand`.
   * Si el `Deck` está vacío, se puede:
     * Simplemente no robar, que la opción por defecto, o
     * Definir una penalización, por ejemplo, perder la `Game`.
   * Esta decisión se toma al inicio del `Game`.

3. `Main Phase`

   * El `ActivePlayer` puede:
     * Jugar `Creature` desde la `Hand` al `Board`.
     * Jugar `Spell`; sus efectos se resuelven y luego van al `Graveyard`.
     * Jugar `Upgrade`, equipándola a una `Creature` o a un `Player`.
   * Cada `Card` jugada debe respetar su `Cost` y el `Ether` disponible ese turno.

4. `Combat Phase`

    * El `ActivePlayer` puede declarar ataques con alguna de sus `Creature`.
    * El `DefenderPlayer` defensor puede declarar una `Creature` bloqueadora
      para cada `Creature` atacante, o puede no bloquear el ataque.
    * Cada `Creature` atacante hace daño directo a la `Creature` del
      `DefenderPlayer` que éste definió.
    * El daño se resuelve comparando `AttackEffective` y `DefenseEffective` de
      las `Creature`: si `AttackEffective` ≥ `DefenseEffective`, la `Creature`
      muere pero el daño remanente no pasa al `DefenderPlayer`; en caso
      contrario, el daño desaparece al final del turno.
    * Las `Creature` no bloqueadas hacen `AttackEffective` puntos de daño al
      `DefenderPlayer`.

    Para calcular el daño efectivo se usan `Dice`, ver [sección
    6.3](#63-cálculo-de-daño-con-dice).

5. `End Phase`

    * Se resuelven efectos que digan “al final de tu turno”.
    * Si al final del turno el `ActivePlayer` tiene más de 7 cartas en la
      `Hand`, debe elegir y descartar cartas al `Graveyard` hasta tener 7.

Luego de la `End Phase`, el turno pasa al otro `Player`.

## 5. Recursos y `Cost` de `Card`

Para limitar la cantidad y el poder de las `Card` que un jugador puede usar en
un mismo turno, cada `Card` tiene un `Cost`.

* Cada Player posee un recurso abstracto llamado `Ether`.
* Al inicio de su turno, el `Ether` disponible del `Player` se establece en 3;
el `Ether` que no se usa se pierde.
* Al jugar una `Card`, se reduce el `Ether` disponible ese turno según el `Cost`
  of the `Card`.
* Ciertos efectos de cartas pueden otorgar `Ether` adicional de forma temporal
  para el turno en curso.
* Cuando el `Ether` de ese turno se agota, el `Player` no puede jugar más `Card`
  hasta su próximo turno.

## 6. Reglas de combate y uso de `Dice`

### 6.1 Estadísticas de `Creature`

Cada `Creature` tiene al menos:

* `AttackBase`: fuerza base.
* `DefenseBase`: resistencia base, o “vida” de la `Creature`.

Estos valores pueden ser modificados temporal o permanentemente por `Upgrade` y
`Spell`. En combate se utiliza un `AttackEffective` y un `DefenseEffective` que
combinan los valores base, modificadores y los resultados de los `Dice`.

### 6.2 Declaración de ataque y resolución de combate

1. Durante la `Combat Phase`, el `ActivePlayer` declara qué `Creature` de su
   `Board` van a realizar un `Attack`.
2. Para cada `Creature` atacante, el `DefenderPlayer` puede declarar una
   `Creature` bloqueadora de su propio `Board`, o decidir no bloquear el ataque.
3. Se calcula el `AttackEffective` de cada atacante y el `DefenseEffective` de
   cada bloqueadora usando los `Dice` correspondientes, como se establece en la
   [sección 6.3](#63-cálculo-de-daño-con-dice).
4. Se resuelve el daño de forma individual por cada pareja o criatura libre: si
   la criatura no fue bloqueada, el `DefenderPlayer` pierde puntos de `Life`
   iguales al `AttackEffective` de esa `Creature`; si la criatura fue bloqueada
   y `AttackEffective` ≥ `DefenseEffective`, la `Creature` bloqueadora es
   destruida y va al `Graveyard`, junto con los `Upgrade` que tuviera; el daño
   remanente se disipa y no afecta a la `Life` del `DefenderPlayer`; en caso
   contrario, la bloqueadora sobrevive y el daño desaparece al final del turno.

### 6.3 Cálculo de daño con `Dice`

El combate usa `Dice` para introducir azar en el cálculo del ataque, la defensa,
o ambos.

* Para cada `Creature` atacante, se lanza un `StandardNumericDice`.
* El `AttackEffective` de la `Creature` se calcula como:

  `AttackEffective = AttackBase + DiceResult`

  * Cada `Creature` defensora también lanza un `StandardNumericDice`.
  * El `DefenseEffective` se calcula como:

    `DefenseEffective = DefenseBase + DiceResult`

  * Cada par atacante–bloqueador compara `AttackEffective` del atacante y
    `DefenseEffective` del defensor; si el `AttackEffective` ≥
    `DefenseEffective`, el defensor es destruido y va al `Graveyard`, junto con
    los `Upgrade` que tuviera.

Otras `Card` podrán modificar estas reglas base.

## 7. `Spell`, `Upgrade` y efectos

Muchas `Card`, sobre todo `Spell` y algunas `Creature` o `Upgrade`, tienen
`Effects` cuando se juegan o mientras están en el `Board`.

Ejemplos de efectos posibles:

* Infligir daño a un `Player` o a una `Creature`.
* Curar `Life`.
* Permitir robar `Card` adicionales.
* Modificar `AttackBase` o `DefenseBase`, temporal o permanentemente.
* Interactuar con `Dice`: `ReRoll`, sumar, restar, elegir el mayor de varios
  resultados, etc.
* Interpretar símbolos de un `RuneDice` para producir distintos outcomes.

A nivel conceptual, un `Effect`:

* Tiene una `Description` en lenguaje natural.
* Se ejecuta cuando una `Card` se juega o cuando se dispara una habilidad,
  aplicando cambios en la `Game`: modificar `Life`, mover `Card` entre zonas,
  etc.

## 8. Dados especiales en `Spell` y `Abilities`

Además del combate estándar, muchas `Spell` y `Abilities` de `Creature` o
`Upgrade` utilizan tipos específicos de `Dice`. El texto de la `Card` siempre
indicará qué `Dice` se debe usar y cómo se interpreta el resultado.

Ejemplos de patrones de uso:

1. Daño inestable con `RiskDice`

   * Se lanza un `RiskDice`.
   * Valores bajos pueden indicar que la `Spell` falla o tiene efecto reducido.
   * Valores altos generan un daño muy elevado.

2. Curación variable con `HealingDice`

   * Se lanza un `HealingDice`.
   * Cada cara del `Dice` corresponde a una combinación de curación, prevención
     de daño o robo de `Card`.
   * El `Player` aplica el efecto según el `DiceResult`.

3. Invocaciones elementales con `RuneDice`

   * Se lanza un `RuneDice`.
   * Cada símbolo, `Fire`, `Water`, `Earth`, `Air`, `Light` o `Shadow`,
     desencadena un `Effect` diferente, que puede ser daño, curación, mejora de
     `Creature`, robo de `Card`, etc.

En todos los casos, se lanza el `Dice` correspondiente, muestra el `DiceResult`
y aplica los efectos definidos por la `Card`.

## 9. Fin de la `Game`

La `Game` termina cuando se cumple alguna de estas condiciones:

* La `Life` de un `Player` es 0 o menor: ese `Player` pierde, el otro gana.
* (Opcional) Un `Player` no puede robar `Card` cuando debería hacerlo; la implementación puede decidir si esto produce derrota o simplemente no se roba.
* (Opcional) Si ambos `Player` llegan a 0 `Life` al mismo tiempo, la `Game`
  puede declararse empate.

## 10. Ejemplo de set inicial de `Card`

A continuación se define un conjunto inicial de `Card` para ilustrar el juego.
Los nombres de dominio están en inglés, aunque el texto descriptivo esté en
español.

### 10.1 `Creature`

#### `Apprentice Warrior`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Una `Creature` |
| `Cost`        | 1              |
| `AttackBase`  | 1              |
| `DefenseBase` | 2              |
| `Description` | Un aprendiz sin habilidades especiales. |
| `Abilities`   | ninguna.       |
<!-- markdownlint-enable MD060 -->

#### `Rune Soldier`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Una `Creature` |
| `Cost`        | 2              |
| `AttackBase`  | 2              |
| `DefenseBase` | 2              |
| `Description` | Un guerrero versátil. |
| `Abilities`   | ninguna.       |
<!-- markdownlint-enable MD060 -->

#### `Spark Mage`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Una `Creature` |
| `Cost`        | 3              |
| `AttackBase`  | 1              |
| `DefenseBase` | 3              |
| `Description` | Cuando `Spark Mage` entra al `Board`, hace 1 punto de daño a cualquier criatura o jugador objetivo. |
| `Ability`     | `OnEnterBattlefield`: hace 1 punto de daño a un `Player` o `Creature` objetivo. |
<!-- markdownlint-enable MD060 -->

#### `Unstable Beast`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Una `Creature`        |
| `Cost`        | 3     |
| `AttackBase`  | 2     |
| `DefenseBase` | 2     |
| `Description` | Cuando ataca, reemplaza su `StandardNumericDice` por un `PowerDice` para calcular su ataque efectivo. |
| `Ability`     | `OnAttack`: reemplaza el dado estándar de combate por un `PowerDice`. |
<!-- markdownlint-enable MD060 -->

#### `Stone Giant`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Una `Creature`        |
| `Cost`        | 4     |
| `AttackBase`  | 3     |
| `DefenseBase` | 5     |
| `Description` | Lento pero resistente. |
| `Abilities`   | ninguna. |
<!-- markdownlint-enable MD060 -->

### 10.2 `Spell`

#### `Unstable Bolt`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Un `Spell` |
| `Cost`        | 2     |
| `Description` | Lanza un `RiskDice`. Si el resultado es 0: este `Spell` falla y no hace nada. Si el resultado es 3 o 4: inflige 2 puntos de daño a cualquier objetivo. Si el resultado es 7 u 8: inflige 4 puntos de daño a cualquier objetivo. |
| `Effect`      | Daño directo dependiente del resultado del `RiskDice`. |
<!-- markdownlint-enable MD060 -->

#### `Minor Heal`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Un `Spell`     |
| `Cost`        | 2              |
| `Description` | Ganas 3 puntos de vida. |
| `Effect`      | El `Player` objetivo gana 3 `Life`. |
<!-- markdownlint-enable MD060 -->

#### `Ether Surge`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| `Cost`        | 1              |
| Qué es        | Un `Spell`     |
| `Description` | Agrega 2 puntos de `Ether` temporal a tu reserva de este turno. Luego lanza un `StandardNumericDice`. Si el resultado es 5 o 6, roba 1 `Card`. |
| `Effect`      | Incrementa el `Ether` disponible en +2. Si sale 5 o 6 en el dado, se añade una carta a la mano. |
<!-- markdownlint-enable MD060 -->

#### `Targeted Destruction`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Un `Spell`     |
| `Cost`        | 3              |
| `Description` | Destruye una `Creature` objetivo con `DefenseBase` 3 o menos. |
| `Effect`      | La `Creature` objetivo que cumpla la condición va al `Graveyard`. |
<!-- markdownlint-enable MD060 -->

#### `Rune Invocation`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Un `Spell`     |
| `Cost`        | 3              |
| `Description` | Lanza un `RuneDice`. |
|               | 🔥 o `Fire`: infliges 3 puntos de daño a cualquier objetivo. |
|               | 💧 o `Water`: ganas 3 puntos de vida. |
|               | 🌱 o `Earth`: una `Creature` que controlas obtiene +2 de defensa hasta el final del turno. |
|               | 🌪️ o `Air`: roba 2 `Card` y luego descarta 1. |
|               | 🌟 o `Light`: ganas 2 `Life` y tu oponente pierde 2 `Life`. |
|               | 🌘 o `Shadow`: cada `Player` pierde 2 `Life`. |
| `Effect`      | Múltiples resultados según el símbolo obtenido en el `RuneDice`. |
<!-- markdownlint-enable MD060 -->

### 10.3 `Upgrade`

#### `Fortune Blade`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Un `Upgrade`, un `Equipment` para `Creature` |
| `Cost`        | 2              |
| `Description` | La `Creature` equipada obtiene +1 `AttackBase`. Cuando ataca, puede volver a lanzar el `StandardNumericDice` de combate y quedarse con el resultado mayor. |
| `Effect`      | +1 `AttackBase` permanente a la `Creature` equipada. En combate, permite un `ReRoll` del `StandardNumericDice` de ataque y tomar el mayor. |
<!-- markdownlint-enable MD060 -->

#### `Rune Shield`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Un `Upgrade`, un `Equipment` para `Creature` |
| `Cost`        | 2     |
| `Description` | La `Creature` equipada obtiene +2 `DefenseBase`. |
| `Effect`      | +2 `DefenseBase` permanente. |
<!-- markdownlint-enable MD060 -->

#### `Protection Aura`

<!-- markdownlint-disable MD060 -->
| Atributo      | Valor          |
| ------------- | -------------- |
| Qué es        | Un `Upgrade` para `Player` |
| `Cost`        | 3                          |
| `Description` | Mientras `Protection Aura` esté en el `Board`, reduce en 1 el daño total recibido por ataques o hechizos la primera vez que se registre daño en el turno. |
| `Effect`      | Reducción de daño una vez por turno para el `Player` que controla esta `Upgrade`. |
<!-- markdownlint-enable MD060 -->

Este set inicial cubre:

* `Creature` simples y `Creature` con efectos al entrar o al atacar.
* `Spell` de daño, curación, robo y destrucción condicionada.
* `Spell` y `Upgrade` que interactúan con distintos tipos de `Dice` (`StandardNumericDice`, `RiskDice`, `PowerDice`, `RuneDice`).
* `Upgrade` que modifican tanto `Creature` como `Player`.
