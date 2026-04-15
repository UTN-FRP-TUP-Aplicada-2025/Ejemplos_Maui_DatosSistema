# ?? WebView Blocking Test - Guía de Uso

## Descripción
Este proyecto incluye un sistema de pruebas para provocar bloqueos y deadlocks en WebView de MAUI, permitiendo verificar cómo Android maneja estas situaciones extremas.

## Archivos Creados

### 1. **WebAPI_DatosSistema/wwwroot/blocking-test.html**
- Página web con 7 tipos diferentes de pruebas de bloqueo
- Interfaz interactiva con botones para cada tipo de test
- Logs en tiempo real

### 2. **Ejemplo_ReportSystemData/Pages/WebViewPage.xaml.cs**
- Actualizado para cargar el HTML bloqueante desde la WebAPI
- URL para Android Emulator: `http://10.0.2.2:5000/blocking-test`
- URL para dispositivo real: `http://localhost:5000/blocking-test`
- Fallback a HTML inline si el servidor no está disponible

### 3. **WebAPI_DatosSistema/Program.cs**
- Configurado para servir el archivo HTML bloqueante
- Endpoint: `/blocking-test`
- CORS habilitado para acceso desde MAUI

## Cómo Usar

### Opción A: Desde Android Emulator

1. **Iniciar la WebAPI:**
   ```bash
   cd WebAPI_DatosSistema
   dotnet run
   ```
   El servidor se inicia en `http://localhost:5000`

2. **Ejecutar la app MAUI:**
   ```bash
   cd Ejemplo_ReportSystemData
   dotnet run -f net10.0-android
   ```

3. **Presionar el botón "Page Test WebView"** en la app

4. **Seleccionar una prueba de bloqueo:**
   - ?? **Infinite Loop**: Bloquea completamente el thread JS
   - ?? **Deep Recursion**: Provoca Stack Overflow
   - ?? **Memory Hog**: Agota la memoria del WebView
   - ?? **Busy Wait**: CPU al 100% durante 5 minutos
   - ?? **Deadlock Simulation**: Simula una condición de deadlock
   - ?? **Promise Chain**: Explota la cadena de promesas
   - ?? **Infinite Event Loop**: DOM thrashing continuo

### Opción B: Desde Dispositivo Real (Android)

1. **Iniciar la WebAPI con acceso externo:**
   ```bash
   cd WebAPI_DatosSistema
   dotnet run --urls "http://0.0.0.0:5000"
   ```

2. **Obtener la IP local del servidor:**
   ```bash
   ipconfig getifaddr en0  # macOS
   ipconfig                 # Windows (buscar IPv4)
   ```

3. **Modificar WebViewPage.xaml.cs** para usar la IP del servidor:
   ```csharp
   private const string BLOCKING_TEST_URL_DEVICE = "http://YOUR_SERVER_IP:5000/blocking-test";
   ```

4. **Ejecutar la app en el dispositivo:**
   ```bash
   dotnet run -f net10.0-android
   ```

## Tipos de Pruebas Disponibles

### 1. **Infinite Loop** ?? CRÍTICO
- Bloquea el thread JavaScript indefinidamente
- El WebView se congela completamente
- Android debería detectarlo y terminar el proceso

### 2. **Deep Recursion**
- Crea una pila de llamadas profunda (50,000 niveles)
- Provoca `RangeError: Maximum call stack size exceeded`
- Consume mucha memoria

### 3. **Memory Hog**
- Asigna 100,000 arrays de 10,000 elementos cada uno
- Cada elemento es una string de 1,000 caracteres
- Puede causar crash por agotamiento de memoria

### 4. **Busy Wait**
- Loop vacío durante 5 minutos (300,000 ms)
- CPU al 100% sin hacer trabajo útil
- Simula trabajo CPU-intensivo bloqueante

### 5. **Deadlock Simulation**
- Simula dos threads compitiendo por locks
- Provoca una condición de espera circular
- Bloquea indefinidamente

### 6. **Promise Chain Explosion**
- Crea una cadena de 100,000 promesas anidadas
- Causa crecimiento exponencial de memoria
- Saturación del Event Loop

### 7. **Infinite Event Loop**
- DOM thrashing continuo
- Crea y agrega elementos DOM en loop infinito
- Causa lag severo y consumo de recursos

## Qué Esperar

### Comportamiento Normal (Sin Bloqueo)
? La UI responde normalmente
? Puedes volver atrás
? Los logs se actualizan

### Comportamiento con Bloqueo
? WebView se congela
? No puedes interactuar
? Los logs se detienen
? Android detecta ANR (Application Not Responding)
? Posible crash o cierre forzado de la app

## Debugging

### Ver logs en Android Studio
```bash
adb logcat | grep "Ejemplo_ReportSystemData"
```

### Ver logs en Visual Studio
1. Abre **Debug > Windows > Output**
2. Selecciona el emulador/dispositivo
3. Observa los logs en tiempo real

### Capturar crash
```bash
adb logcat > crash_log.txt
# Presionar botón de bloqueo
# Esperar ANR
# Ctrl+C
```

## Configuración del Servidor Web

El servidor web está configurado en **WebAPI_DatosSistema/Program.cs**:

```csharp
// CORS habilitado para todas las solicitudes
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Endpoint que sirve el HTML
app.MapGet("/blocking-test", async (HttpContext context) => { ... });
```

## Notas Importantes

1. **10.0.2.2 para Android Emulator**: Este es el alias para `localhost` en el emulador Android
2. **CORS**: Está habilitado para permitir acceso cross-origin desde la app MAUI
3. **Fallback HTML**: Si el servidor no está disponible, se carga un HTML simplificado inline
4. **No es para Producción**: Este código es SOLO para testing de comportamiento ante bloqueos

## Posibles Mejoras Futuras

- [ ] Agregar endpoint que inicie bloqueos sin necesidad de UI
- [ ] Agregar métricas de consumo de CPU/Memoria
- [ ] Crear versión con diferentes configuraciones de timeout
- [ ] Agregar crash reporter integrado

## Referencias

- [MAUI WebView Documentation](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/webview)
- [Android ANR (Application Not Responding)](https://developer.android.com/topic/performance/anr)
- [JavaScript Performance](https://developer.mozilla.org/en-US/docs/Web/Performance)
