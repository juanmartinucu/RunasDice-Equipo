<!-- omit in toc -->
# Consigna proyecto 2026 2º semestre: historias de usuario

<!-- markdownlint-disable-next-line MD033 -->
<img alt="Logo" src="./assets/Runas_and_Dices.png" width="500">

Una [historia de usuario o *user
story*](https://es.wikipedia.org/wiki/Historias_de_usuario) es una descripción
corta y sencilla de una necesidad de una persona que utiliza la aplicación,
escrita desde su punto de vista, que sirve para guiar el desarrollo de esa
aplicación.

A continuación de damos la lista de historias de usuario a implementar:

1. Como `Player`, quiero unirme a la lista de espera por un oponente.

    Criterios de aceptación:

    * El `Player` recibe un mensaje confirmando que fue agregado a la lista de
      espera.

2. Como `Player`, quiero ver la lista de `Player` esperando por un oponente.

    Criterios de aceptación:

    * En la pantalla se ve la lista de los `Player` que se unieron a la lista de
      espera.

3. Como `Player`, quiero iniciar una partida con un `Player` que está esperando
   por un oponente.

    Criterios de aceptación:

    * Ambos `Player` son notificados del inicio de la partida.
    * El `Player` que tiene el primer turno se determina aleatoriamente.

4. Como `Player`, quiero que la aplicación configure la partida automáticamente
   tras ser emparejado.

    Criterios de aceptación:

    * Ambos `Player` comienzan con un `Deck` inicial de 30 cartas barajado
      aleatoriamente.
    * Ambos `Player` roban automáticamente su `StartingHand` de 5 cartas.
    * La `Life` de ambos `Player` se inicializa en 20.
    * la aplicación asigna claramente el rol de `ActivePlayer` al `Player`
      seleccionado al azar y de `DefenderPlayer` al oponente.

5. Como `ActivePlayer`, quiero iniciar mi turno cargando mis recursos y
   robando una carta.

    Criterios de aceptación:

    * Al inicio del turno (`Start Phase`), se resuelven los efectos
      correspondientes y se limpian los estados temporales del turno anterior.
    * En la `Draw Phase`, la reserva de `Ether` disponible se establece
      exactamente en 3 de forma fija (el sobrante no usado se pierde).
    * la aplicación añade automáticamente la carta superior del `Deck` a la `Hand`.
    * Si el `Deck` está vacío al robar, se aplica la regla de penalización
      acordada al inicio de la partida (por ejemplo, no robar o derrota
      instantánea).

6. Como `ActivePlayer`, quiero jugar una carta de tipo `Creature` de mi mano
   pagando su costo de Ether.

    Criterios de aceptación:

    * Puedo seleccionar una carta de tipo `Creature` de mi `Hand` durante la
      `Main Phase`.
    * Solo puedo jugarla si mi `Ether` disponible es mayor o igual al `Cost` de
      la carta.
    * Al jugarla, el `Ether` disponible se reduce según el costo de la carta y
      la criatura se mueve al `Board`.
    * Si la criatura tiene una habilidad de entrada al campo de batalla
      (`OnEnterBattlefield`), la aplicación la resuelve automáticamente. Si la
      habilidad requiere objetivo, la aplicación solicita elegir un objetivo válido
      (`Player` o `Creature`) y aplica el efecto definido por la carta (por
      ejemplo, infligir daño, curar, modificar atributos, robar cartas, etc.).

7. Como `ActivePlayer`, quiero jugar una carta de tipo `Spell` o una de tipo
   `Upgrade` para alterar el estado del juego.

    Criterios de aceptación:

    * Puedo jugar una carta `Spell` pagando su `Cost` en `Ether`. Su efecto se
      resuelve de inmediato y la carta va al `Graveyard`; por ejemplo, un
      hechizo que otorgue `Life`, modifique atributos o genere otros efectos
      sobre un objetivo.
    * Puedo jugar una carta `Upgrade` pagando su `Cost` y equipar con ella un
      objetivo válido en el `Board`: `Creature` o `Player`.
    * la aplicación valida las condiciones de objetivo definidas por cada carta
      (por ejemplo, solo permitir destruir criaturas con cierta `DefenseBase`
      máxima) antes de aplicar el efecto.
    * Al jugar cartas con efectos asociados a dados, la aplicación lanza
      automáticamente el tipo de dado indicado en la carta
      (`StandardNumericDice`, `RiskDice`, `RuneDice` u otros definidos) y
      procesa el resultado según la descripción de la carta.

8. Como `ActivePlayer`, quiero declarar qué criaturas van a atacar en la Fase
   de Combate.

    Criterios de aceptación:

    * Al entrar en la `Combat Phase`, puedo seleccionar cuáles de mis `Creature`
      en el `Board` realizarán un `Attack`.
    * la aplicación notifica al `DefenderPlayer` cuáles son las criaturas atacantes
      y espera su declaración de defensa.

9. Como `DefenderPlayer`, quiero asignar mis criaturas bloqueadoras para
   defenderme de los ataques.

    Criterios de aceptación:

    * Durante la `Combat Phase`, puedo asignar una `Creature` de mi `Board` para
      bloquear a cada criatura atacante declarada, o decidir no hacerlo.
    * Una criatura bloqueadora propia solo puede interceptar a una única
      criatura atacante.

10. Como `Player`, quiero que la aplicación resuelva los combates usando los dados
    automáticos.

    Criterios de aceptación:

    * Para cada par Atacante–Bloqueador, la aplicación lanza un
      `StandardNumericDice` para cada criatura y calcula: `AttackEffective =
      AttackBase + DiceResult` y `DefenseEffective = DefenseBase + DiceResult`.
    * Si la criatura atacante tiene una habilidad que modifica el dado de ataque
      (por ejemplo, reemplazarlo por otro tipo de dado como un `PowerDice` o
      similar), la aplicación aplica esa sustitución de forma automática.
    * Si la criatura atacante tiene una mejora o habilidad que modifica su
      ataque base o la forma de lanzar dados (por ejemplo, sumar a su
      `AttackBase`, lanzar múltiples dados y elegir el mayor, o aplicar otros
      modificadores), la aplicación aplica estas reglas antes de comparar
      resultados.
    * Si `AttackEffective ≥ DefenseEffective`, la criatura defensora es
      destruida y va al `Graveyard` con sus `Upgrade`. En caso contrario,
      sobrevive y el daño remanente desaparece al terminar el turno.
    * Las criaturas atacantes que no fueron bloqueadas infligen daño directo
      igual a su `AttackEffective` a la `Life` del `DefenderPlayer`.
    * Si el `DefenderPlayer` tiene equipada una mejora defensiva que reduce o
      mitiga daño recibido, la aplicación aplica esa reducción la primera vez que
      registre daño en ese turno, según las reglas de dicha carta.

11. Como `ActivePlayer`, quiero finalizar mi turno de forma manual y controlar
    el límite de mi mano.

    Criterios de aceptación:

    * Puedo declarar voluntariamente el fin de mi turno para pasar a la `End
      Phase`.
    * Si tengo más de 7 cartas en la `Hand`, la aplicación me obliga a elegir y
      descartar cartas al `Graveyard` hasta tener exactamente 7.
    * Una vez resuelta la fase, el turno termina y el rol de `ActivePlayer` pasa
      al oponente.

12. Como `Player`, quiero que la aplicación declare el fin de la partida cuando la
    vida de alguien llegue a 0 o menos.

    Criterios de aceptación:

    * En el momento en que la `Life` de cualquier `Player` es 0 o menor, el
      juego detiene cualquier acción posterior inmediatamente.
    * la aplicación anuncia al ganador en el chat bot y actualiza el estado general
      de la partida (`Game`) a finalizada.
