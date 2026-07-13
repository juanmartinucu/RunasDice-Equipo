# Consigna proyecto 2026 2º semestre: bot de Discord <!-- omit in toc -->

<!-- markdownlint-disable-next-line MD033 -->
<img alt="Logo" src="./assets/Runas_and_Dices.png" width="500">

<!-- markdownlint-disable-next-line MD025 -->
# Tabla de contenido <!-- omit in toc -->

* [Bot de Discord](#bot-de-discord)
  * [Crear y probar el bot](#crear-y-probar-el-bot)
  * [Comandos del bot](#comandos-del-bot)

<!-- markdownlint-disable-next-line MD025 -->
# Bot de Discord

El proyecto consiste en crear un bot de Discord para jugar `Runas & Dices`. En
este documento te explicamos cómo crear ese bot.

> [!TIP]
> Ten en cuenta que el código que te damos ya tiene un bot implementado con
> algunas historias de usuario, tal como te explicamos [aquí](./SEED.md).

## Crear y probar el bot

Para crear y probar el bot:

1. Crea un servidor en Discord.

2. Crea un nuevo bot en Discord siguiendo [estas
   instrucciones](https://docs.discordnet.dev/guides/getting_started/first-bot.html).

   Anota el token que te muestra la página, porque por seguridad, no podrás
   volver a verlo.

   Marca `Server Members Intent` y `Message Content Intent`.

   Cuando, siguiendo el procedimiento, debas generar la URL para
   registrar el bot en el servidor, usa estas opciones:

   * En `Scopes`, marca `bot`.
   * En `Bot Permissions`, marca `View Channels` en `General Permissions`, y
     `Send Messages` y `Read Message History` en `Text Permissions`.
   * En `Integration Type`, las opciones son `Guid Install` o
   `User Install`, elije `Guid Install`.

3. Usa la URL que generaste en el paso anterior para agregar tu bot al servidor.

4. Crea un archivo `secrets.json` en las siguientes ubicaciones dependiendo de
   tu sistema operativo; si no existe alguna de las carpetas en la ruta
   deberás crearla;`%APPDATA%` en Windows siempre existe, así como `~`
   siempre existe en Linux/macOS-:

   * **Windows**: `%APPDATA%\\Microsoft\\UserSecrets\\RunasDices\\secrets.json`
   * **Linux/macOs**: `~/.microsoft/usersecrets/RunasDices/secrets.json

5. Edita el archivo `secrets.json` para que contenga la configuración que
   aparece a continuación, donde reemplazas `<token>` por el que te dio el
   Discord:

    ```json
    {
        "DiscordToken": "<token>",
    }
    ```

6. Ejecuta tu bot presionando <kbd>F5</kbd> en Visual Studio Code.

> 🤔 ¿Porqué la complicamos con el token?
>
> El token queda en un archivo en tu computadora y no en el código de tu bot. De
> esta forma vas a poder subir el código a repositorios de GitHub sin compartir
> el token, así sigue siendo secreto. El token guardado en el archivo
> `secrets.json` se lee con una API de .NET que maneja los secretos de forma
> segura.

## Comandos del bot

Para jugar, debes enviar comandos al bot. Los comandos son mensajes en un canal
de Discord que comienzan con `!`.

El código que te damos ya tiene programados algunos comandos, que encontrarás en
la carpeta [`Commands`](./src/Library/Commands/).

El bot responde a los siguientes mensajes:

* `!who [username]`: `username` es opcional. Devuelve información sobre el
  usuario que envía el mensaje o sobre el usuario `username`.

* `!play [{username}]`: `username` es opcional. Cuando **no** indicas
  `username`, el comando te agrega a la lista de usuarios esperando para jugar.
  Cuando **sí** indicas `username`, si `username` está esperando para jugar,
  inicia una partida en la que juegas contra `username`.

* `!waitlist`. Deuelve la lista de jugadores esperando por un oponente para
  jugar. Esta es la [historia de usuario](./STORIES.md) número 2.

> [!NOTE]
> Este bot está basado en [este
> tutorial](https://blog.adamstirtan.net/2023/10/create-discord-bot-in-c-and-net-part-1.html).
