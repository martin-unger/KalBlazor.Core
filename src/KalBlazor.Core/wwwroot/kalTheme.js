export function isDarkMode() {
    return document.documentElement.classList.contains('dark');
}

export function setDarkMode(enabled) {
    document.documentElement.classList.toggle('dark', enabled);
}

