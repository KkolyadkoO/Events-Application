import React, {createContext} from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import './css/global.css';
import AppRoutes from './components/route/appRoutes';
import Store from "./store/store";

interface State {
    store: Store;
}

const store = new Store();

export const Context = createContext<State>({
    store,
});

const rootElement = document.getElementById('root');
if (rootElement) {
    const root = ReactDOM.createRoot(rootElement);
    root.render(
        <Context.Provider value={{store}}>
            <AppRoutes />
        </Context.Provider>
    );
} else {
    console.error('Root element not found');
}
