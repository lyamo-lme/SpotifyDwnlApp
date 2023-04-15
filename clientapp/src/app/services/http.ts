export const request = async <T>(url: string, method: "GET" | "POST" | "UPDATE" | "DELETE"): Promise<T> => {
    const response = await fetch(url, {
        method: method
    });
    return await response.json() as T;
};
                        