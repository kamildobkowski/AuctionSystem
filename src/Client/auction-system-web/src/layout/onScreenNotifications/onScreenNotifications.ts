import { notification } from 'antd';

export function showServerError(description?: string) {
    notification.error({
        message: 'Błąd serwera',
        description: description ?? 'Wystąpił błąd serwera.',
        placement: 'topRight',
    });
}

// na samym dole pliku albo w index.tsx:
(window as any).showServerError = showServerError;