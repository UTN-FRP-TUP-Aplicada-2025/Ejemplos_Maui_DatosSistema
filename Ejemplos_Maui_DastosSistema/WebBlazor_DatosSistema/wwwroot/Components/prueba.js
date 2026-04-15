

export function log(msg) {
    const logElement = document.getElementById('log');
    const statusElement = document.getElementById('status');

    const ts = new Date().toLocaleTimeString();
    logElement.innerHTML += `[${ts}] ${msg}<br>`;
    logElement.scrollTop = logElement.scrollHeight;
}

export function updateStatus(msg, type = 'info') {
    const logElement = document.getElementById('log');
    const statusElement = document.getElementById('status');

    const colors = { 'info': '#2196F3', 'warning': '#ff9800', 'error': '#f44336' };
    statusElement.style.borderLeftColor = colors[type];
    statusElement.innerHTML = `<strong>Estado:</strong> ${msg}`;
}

export function infiniteLoop() {
    const logElement = document.getElementById('log');
    const statusElement = document.getElementById('status');

    updateStatus('⚠️ Infinite Loop iniciado...', 'error');
    log('🔴 Iniciando Infinite Loop - Bloqueo Total');
    let c = 0;
    while (true) { c++; }
}

export function recursiveLoop() {
    const logElement = document.getElementById('log');
    const statusElement = document.getElementById('status');

    updateStatus('⚠️ Recursion iniciada...', 'error');
    log('🔴 Iniciando Deep Recursion');
    function recurse(d) { if (d < 50000) recurse(d + 1); }
    try { recurse(0); } catch (e) { log('Stack Overflow'); updateStatus('Stack Overflow', 'error'); }
}

export function memoryHog() {
    const logElement = document.getElementById('log');
    const statusElement = document.getElementById('status');

    updateStatus('⚠️ Memory Hog iniciado...', 'error');
    log('🔴 Iniciando consumo masivo de memoria');
    try {
        const arrays = [];
        for (let i = 0; i < 100000; i++) {
            arrays.push(new Array(10000).fill('x'.repeat(1000)));
            if (i % 1000 === 0) log(`Arrays: ${i}`);
        }
    } catch (e) { log('Out of Memory'); updateStatus('Out of Memory', 'error'); }
}

export function busyWait() {
    const logElement = document.getElementById('log');
    const statusElement = document.getElementById('status');

    updateStatus('⚠️ Busy Wait iniciado...', 'error');
    log('🔴 Iniciando Busy Wait (CPU al 100%)');
    const start = Date.now();
    while (Date.now() - start < 300000) { Math.sqrt(Math.random()); }
}

export function deadlockSimulation() {
    const logElement = document.getElementById('log');
    const statusElement = document.getElementById('status');

    updateStatus('⚠️ Deadlock iniciado...', 'error');
    log('🔴 Iniciando Deadlock Simulation');
    let l1 = false, l2 = false, c1 = 0, c2 = 0;
    while (true) {
        c1++; c2++;
        if (c1 > 1000000 && c2 > 1000000) {
            l1 = !l1; l2 = !l2; c1 = 0; c2 = 0;
        }
    }
}

export function reset() {
    const logElement = document.getElementById('log');
    const statusElement = document.getElementById('status');

    logElement.innerHTML = 'Logs limpios<br>';
    updateStatus('Sistema reseteado', 'info');
    log('Reset completado');
}
