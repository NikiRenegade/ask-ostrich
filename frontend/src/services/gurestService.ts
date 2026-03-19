export const getGuestId = () => {
    let id = localStorage.getItem('guestId');
    if (!id) {
        id = crypto.randomUUID();
        localStorage.setItem('guestId', id);
    }
    return id;
};

export const getGuestName = () => {
    return localStorage.getItem('guestName');
};

export const setGuestName = (name: string) => {
    localStorage.setItem('guestName', name);
};

export const getDisplayName = (user?: { name?: string }) => {
    if (user?.name) return user.name;

    const name = getGuestName();
    if (name) return name;

    const guestId = getGuestId();
    return `Гость ${guestId.slice(0, 4)}`;
};
export const deleteGuest = () => {
    localStorage.removeItem('guestId');
    localStorage.removeItem('guestName');
}