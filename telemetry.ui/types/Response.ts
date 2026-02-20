export type Response<T> = {
    status: number;
    message: string;
    payload: T;
}
