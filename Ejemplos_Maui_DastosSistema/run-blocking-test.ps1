#!/usr/bin/env pwsh
# Script para facilitar el testing de WebView Blocking

param(
    [Parameter(Mandatory=$false)]
    [ValidateSet("server", "app", "both", "logs")]
    [string]$Action = "both",
    
    [Parameter(Mandatory=$false)]
    [string]$Platform = "android",
    
    [Parameter(Mandatory=$false)]
    [switch]$Release
)

$ErrorActionPreference = "Stop"

function Write-Banner {
    param([string]$Message)
    Write-Host ""
    Write-Host "??????????????????????????????????????????" -ForegroundColor Cyan
    Write-Host "? $($Message.PadRight(36)) ?" -ForegroundColor Cyan
    Write-Host "??????????????????????????????????????????" -ForegroundColor Cyan
    Write-Host ""
}

function Start-Server {
    Write-Banner "?? Iniciando WebAPI Server"
    $serverPath = Join-Path $PSScriptRoot "WebAPI_DatosSistema"
    
    if (-not (Test-Path $serverPath)) {
        Write-Host "? WebAPI_DatosSistema no encontrado en: $serverPath" -ForegroundColor Red
        exit 1
    }
    
    Set-Location $serverPath
    Write-Host "?? Directorio: $serverPath" -ForegroundColor Green
    Write-Host ""
    Write-Host "? Servidor iniciando en http://localhost:5000" -ForegroundColor Green
    Write-Host "?? Blocking Test disponible en http://localhost:5000/blocking-test" -ForegroundColor Yellow
    Write-Host ""
    
    if ($Release) {
        Write-Host "?? Compilando en modo Release..." -ForegroundColor Yellow
        dotnet run --configuration Release
    } else {
        dotnet run
    }
}

function Start-App {
    Write-Banner "?? Iniciando MAUI App"
    $appPath = Join-Path $PSScriptRoot "Ejemplo_ReportSystemData"
    
    if (-not (Test-Path $appPath)) {
        Write-Host "? Ejemplo_ReportSystemData no encontrado en: $appPath" -ForegroundColor Red
        exit 1
    }
    
    Set-Location $appPath
    Write-Host "?? Directorio: $appPath" -ForegroundColor Green
    Write-Host "?? Plataforma: $Platform" -ForegroundColor Green
    Write-Host ""
    
    $framework = "net10.0-$Platform"
    
    if ($Release) {
        Write-Host "?? Compilando en modo Release..." -ForegroundColor Yellow
        dotnet run -f $framework --configuration Release
    } else {
        dotnet run -f $framework
    }
}

function Show-Logs {
    Write-Banner "?? Mostrando Logs"
    
    if ($Platform -eq "android") {
        Write-Host "?? Conectando a dispositivo/emulador Android..." -ForegroundColor Green
        Write-Host ""
        Write-Host "Presiona Ctrl+C para detener los logs" -ForegroundColor Yellow
        Write-Host ""
        
        adb logcat --filters "MAUI:V *:S"
    } else {
        Write-Host "??  Logs solo disponibles para Android" -ForegroundColor Yellow
    }
}

# Ejecutar según la acción
switch ($Action) {
    "server" {
        Start-Server
    }
    "app" {
        Start-App
    }
    "both" {
        Write-Host "??  Para ejecutar ambos simultáneamente, abre dos terminales:" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Terminal 1:" -ForegroundColor Cyan
        Write-Host "  .\run-blocking-test.ps1 -Action server" -ForegroundColor White
        Write-Host ""
        Write-Host "Terminal 2:" -ForegroundColor Cyan
        Write-Host "  .\run-blocking-test.ps1 -Action app -Platform $Platform" -ForegroundColor White
        Write-Host ""
        Write-Host "Luego, en la app MAUI:" -ForegroundColor Cyan
        Write-Host "  1. Presiona 'Page Test WebView'" -ForegroundColor White
        Write-Host "  2. Selecciona una prueba de bloqueo" -ForegroundColor White
        Write-Host ""
    }
    "logs" {
        Show-Logs
    }
}

Write-Host ""
