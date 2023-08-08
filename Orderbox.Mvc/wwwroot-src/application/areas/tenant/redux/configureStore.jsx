import { createStore, applyMiddleware } from 'redux';
import { createLogicMiddleware } from 'redux-logic';
import { logger } from 'redux-logger';
import { persistStore, persistReducer } from 'redux-persist';
import storage from 'redux-persist/lib/storage'

import reducers from './reducers';
import logics from './logics';

const persistConfig = {
    key: 'root',
    storage,
    whitelist: ['BuyerReducer', 'CartReducer', 'OrderHistoryReducer']
}

const persistedReducer = persistReducer(persistConfig, reducers);

export default function configureStore() {
    const logicMiddleware = createLogicMiddleware(logics);
    const middlewares = [logicMiddleware]

    if (process.env.NODE_ENV === 'development') {        
        middlewares.push(logger);
    }

    const store = createStore(
        persistedReducer,
        applyMiddleware(...middlewares)
    );

    const persistor = persistStore(store);

    return { store, persistor };
}