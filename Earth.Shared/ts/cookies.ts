export function writeCookie(name: string, value: string): void {
    document.cookie = `${name}=${value};path=/`;
}

export function readCookies(): { [name: string]: string } {
    var ix: { [name: string]: string } = {};
    var allCookies = document.cookie;

    var cookies = allCookies.split(";");

    for (var c of cookies) {
        var parts = c.split("=");
        var name = parts[0]?.trim()?.toLowerCase();
        var val = parts[1]?.trim();

        if (name && val) {
            ix[name] = val;
        }
    }

    return ix;
}

export function readCookie(name: string): string {
    var ix = readCookies();
    return ix[name.toLowerCase()];
}
