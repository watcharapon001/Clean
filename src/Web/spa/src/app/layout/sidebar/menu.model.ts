export interface Menu {
    menuId: string;
    menuCode: string;
    menuName: string;
    route?: string;
    icon?: string;
    sequence: number;
    parentMenuId?: string;
    children?: Menu[];
}
