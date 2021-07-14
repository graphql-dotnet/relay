import 'todomvc-common';
import 'todomvc-app-css/index.css';

import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import registerServiceWorker from './registerServiceWorker';
import AppRoot from './App';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
    <BrowserRouter basename = { baseUrl } >
        <AppRoot />
    </BrowserRouter>, rootElement);

registerServiceWorker();

