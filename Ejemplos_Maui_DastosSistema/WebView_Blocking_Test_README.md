# ?? WebView Blocking Test - Testing de Deadlocks en MAUI

Un conjunto completo de pruebas para provocar bloqueos y deadlocks en WebView de MAUI y observar cómo Android maneja estas situaciones extremas.

## ? Quick Start

### 1?? Terminal 1 - Iniciar Servidor Web
```bash
# Windows
run-blocking-test.bat server

# macOS/Linux
./run-blocking-test.ps1 -Action server
```

### 2?? Terminal 2 - Iniciar App MAUI
```bash
# Windows
run-blocking-test.bat app android

# macOS/Linux
./run-blocking-test.ps1 -Action app -Platform android
```

### 3?? En la App
1. Presiona **"Page Test WebView"** en MainPage
2. Se abre WebViewPage con 7 opciones de bloqueo
3. Selecciona una y observa cómo Android responde

## ?? Tipos de Pruebas Disponibles

| Prueba | Efecto | Comportamiento |
|--------|--------|----------------|
| ?? **Infinite Loop** | Bloqueo total | WebView se congela, Android detecciona ANR |
| ?? **Deep Recursion** | Stack Overflow | Agotamiento de pila, error de recursión |
| ?? **Memory Hog** | Consumo masivo | Agotamiento de RAM, posible OOM crash |
| ?? **Busy Wait** | CPU 100% | Spinning innecesario durante 5 minutos |
| ?? **Deadlock Simulation** | Espera circular | Bloqueo mutuo indefinido |
| ?? **Promise Chain** | Event Loop saturado | Exploración exponencial de memoria |
| ?? **Infinite Event Loop** | DOM thrashing | Creación continua de elementos, lag severo |

## ?? Archivos Creados

```
Proyecto/
??? BLOCKING_TEST_GUIDE.md              ? Guía detallada
??? run-blocking-test.bat               ? Launcher para Windows
??? run-blocking-test.ps1               ? Launcher para PowerShell
?
??? WebAPI_DatosSistema/
?   ??? Program.cs                      ? Endpoint /blocking-test
?   ??? wwwroot/
?       ??? blocking-test.html          ? Página web bloqueante
?
??? Ejemplo_ReportSystemData/
    ??? Pages/
        ??? WebViewPage.xaml.cs         ? Carga HTML desde servidor
```

## ?? Configuración

### URLs según plataforma

- **Android Emulator**: `http://10.0.2.2:5000/blocking-test`
- **Dispositivo Real**: `http://[SERVER_IP]:5000/blocking-test`
- **iOS/Mac**: `http://localhost:5000/blocking-test`

### Modificar URL en WebViewPage.xaml.cs

```csharp
private const string BLOCKING_TEST_URL = "http://10.0.2.2:5000/blocking-test";
private const string BLOCKING_TEST_URL_DEVICE = "http://localhost:5000/blocking-test";
```

## ?? Qué Observar

### Métrica Normal (sin bloqueo)
? WebView responde  
? Logs actualizados  
? Puedes volver atrás  

### Métrica Anormal (con bloqueo)
? WebView congelado  
? Logs detenidos  
? Android detecta ANR  
? Posible crash forzado  

## ?? Debugging

### Ver logs en tiempo real
```bash
# Android
adb logcat | grep -i "maui\|webview\|anr"

# O desde Visual Studio
# Debug > Windows > Output > Emulator/Device
```

### Capturar crash
```bash
adb logcat > blocking_test.log
# (Presionar botón de bloqueo)
# (Esperar ANR)
# Ctrl+C
```

## ?? Arquitectura

```
???????????????????????????????????????
?   MAUI App (Ejemplo_ReportSystemData)?
?         WebViewPage                 ?
?           ?                         ?
?           ?? Carga URL:             ?
?           ?  http://10.0.2.2:5000   ?
?           ?                         ?
?           ??? HTTP GET              ?
???????????????????????????????????????
              ?
              ? HTTP/1.1
              ?
???????????????????????????????????????
?    WebAPI (WebAPI_DatosSistema)     ?
?   /blocking-test endpoint           ?
?   Sirve HTML bloqueante             ?
???????????????????????????????????????
```

## ?? Configuración del Servidor

**Program.cs** incluye:
- ? CORS habilitado
- ? Servicio de archivos estáticos
- ? Endpoint `/blocking-test`
- ? Fallback a HTML inline

```csharp
app.MapGet("/blocking-test", async (HttpContext context) => 
{
    await context.Response.SendFileAsync(filePath);
});
```

## ?? Aprendizajes Esperados

1. **Bloqueos JavaScript**: Impacto en WebView
2. **ANR (Application Not Responding)**: Detección por Android
3. **Gestión de memoria**: Límites de WebView
4. **Debugging de WebView**: Herramientas y técnicas
5. **Comportamiento de degradación**: Cómo responde la app

## ?? Advertencias

- ? **NO usar en producción**
- ? **Puede causar crash de la app**
- ? **Puede congelar el emulador**
- ? **Alto consumo de recursos**

## ?? Referencias

- [MAUI WebView](https://learn.microsoft.com/dotnet/maui/user-interface/controls/webview)
- [Android ANR](https://developer.android.com/topic/performance/anr)
- [JavaScript Performance](https://developer.mozilla.org/docs/Web/Performance)
- [Chrome DevTools WebView Debug](https://developer.chrome.com/docs/devtools/)

## ?? Soporte

Para problemas:

1. Verifica que el servidor esté corriendo en puerto 5000
2. Confirma la URL correcta según la plataforma
3. Revisa los logs con `adb logcat`
4. Prueba con el fallback HTML inline

---

**Creado**: 2025  
**Versión**: 1.0  
**Estado**: Testing y desarrollo
