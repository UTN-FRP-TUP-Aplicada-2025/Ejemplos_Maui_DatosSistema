
## Comandos útiles de logcat

```
// Solo errores y warnings
$"logcat -d -t 200 --pid={pid} *:E *:W"

// Solo errores
$"logcat -d -t 200 --pid={pid} *:E"

// Últimas 500 líneas
$"logcat -d -t 500 --pid={pid}"

// Solo de tu app con tag específico
$"logcat -d --pid={pid} YourAppTag:V *:S"

// Con formato de tiempo
$"logcat -d -v time --pid={pid}"

// Con formato detallado
$"logcat -d -v long --pid={pid}"
```