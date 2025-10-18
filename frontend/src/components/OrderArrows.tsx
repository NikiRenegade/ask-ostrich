import { reorder } from "../services/reorder.ts"

interface OrderArrowsProps<T extends { Order: number }> {
    item: T;
    list: T[];
    setList: (item: T) => void;
}

export const OrderArrows = <T extends { Order: number }>
                                                            ({item, list, setList,}: OrderArrowsProps<T>) => {
    const moveUp = () => {
        const newList = reorder(list, item.Order, 1); // шаг назад = 1
        if (newList) setList(item);
    };

    const moveDown = () => {
        const newList = reorder(list, item.Order, -1); // шаг вперёд = -1
        if (newList) setList(item);
    };

    return (
        <div className="flex flex-col items-center space-y-1">
            {item.Order !== 1 && <button
                type="button"
                className="text-gray-500 hover:text-gray-700 leading-none"
                onClick={moveUp} disabled={item.Order === 1}
                title="Переместить вверх">↑</button> }
            {item.Order !== list.length && <button
                type="button"
                className="text-gray-500 hover:text-gray-700 leading-none"
                onClick={moveDown} disabled={item.Order === list.length}
                title="Переместить вниз">↓</button>}
        </div>
    );
};