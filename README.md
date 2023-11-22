# Proyecto de Servicio Social [TUTORIAL Listo con la Lista]

## Introducción
El juego Listo con la Lista (AKA "el del supermercado"), es un juego de memoria que utiliza recursos visuales para memorizar una secuencia de artículos de supermercado y despensa.

## Dinámica de Juego

Listo con la Lista tiene dos fases principales. En la primera se ilustra el hogar del jugador, en particular su despensa, en la cual hacen falta algunos artículos de uso diario (comida, limpieza, artículos para el baño, etc). El jugador debe recordar cuáles de los artículos resaltados por un círculo de luz necesita comprar en la tienda.

La segunda fase del juego se desarrolla en el supermercado, donde una gran gama de artículos se desplegará. Entre estos están siempre aquellos que el jugador debe comparar. Al terminar de seleccionar todos ellos (sin importar el orden) el jugador termina sus compras.

El juego evalúa la respuesta del jugador, dependiendo si este logró superar la pantalla, el juego maneja de forma dinámica el nivel del jugador, pudiendo subir de nivel o bajar.

## Consideraciones
El sistema de nivel dinámico se basa en el número de victorias o derrotas de un nivel particular:

- 3 victorias seguidas, el jugador subirá un nivel.
- 3 derrotas seguidas, el jugador bajará un nivel.
- Perder tras tener un stack de victorias, pierde el stack de victorias.
- Ganar tras tener un stack de derrotas, pierde el stack de derrotas.

## Estructura General del juego
El juego se compone de 10 niveles, cada uno sube un poco la dificultad cambiando el número de artículos a recordar y los distractores. En todos los niveles se tienen 2 minutos para memorizar y 2 minutos para comprar.
1. Memorizar: 2, Distractores: 0 (Ayuda de cuenta)
2. Memorizar: 3, Distractores: 0 (Ayuda de cuenta)
3. Memorizar: 2, Distractores: 2 (Ayuda de cuenta)
4. Memorizar: 3, Distractores: 2 (Ayuda de cuenta)
5. Memorizar: 4, Distractores: 3 (Ayuda de cuenta)
6. Memorizar: 5, Distractores: 2
7. Memorizar: 6, Distractores: 3
8. Memorizar: 6, Distractores: 4
9. Memorizar: 7, Distractores: 5
10. Memorizar: 7, Distractores: 6

## Tutorial
El tutorial ilustrará las mecánicas principales del juego (fase 1 y fase 2) además de explicar el sistema dinámico de niveles (condiciones de victoria y derrota).

## Registro de Actividades

### 21-11-23
Redaccion y analisis te estructura del juego y condiciones de victoria. Se creo el repositorio del proyecto, ademas se hicieron pruebas del funcionamiento del juego y se analizaron las caracteristicas de todos los niveles.

### 22-11-23
Se terminaron de hacer los assets necesarios para la explicación tutorial. También se creó la escena tutorial con los elementos para desplegar los mensajes. Se modificaron algunos assets para hacer diferente visualmente el tutorial. Se cambió el nombre de algunos assets no relacionados con el tutorial.
