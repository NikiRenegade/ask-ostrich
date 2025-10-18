export function reorder<T extends {Order : number}>(list: T[], order: number, step: number) : T[] | undefined {
    const newList = [...list];
    const current = newList.find(t => t.Order === order);
    if (!current) return newList;
    const targetOrder = order - step;
    if (targetOrder < 1 || targetOrder > newList.length) return newList;
    const other = newList.find(t => t.Order === targetOrder);
    if (!other) return newList;
    current.Order = targetOrder;
    other.Order = order;
    return newList.sort((a, b) => a.Order- b.Order);
}