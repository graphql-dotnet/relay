import React, { Component } from 'react';
import graphql from 'babel-plugin-relay/macro';
import {
    RelayEnvironmentProvider,
    loadQuery,
    usePreloadedQuery,
} from 'react-relay/hooks';
import RelayEnvironment from './RelayEnvironment';

import Header from "./components/Header";
import MainSection from "./components/MainSection";
import TodoApp from './components/TodoApp';

const { Suspense } = React;

const AppQuery = graphql`
  query AppQuery {
    viewer {
        ...TodoApp_viewer
    }
  }
`;

const preloadedQuery = loadQuery(RelayEnvironment, AppQuery, {
    /* query variables */
});

function App(props) {
    const displayName = App.name;
    const data = usePreloadedQuery(AppQuery, props.preloadedQuery);
    console.log(data);
    return (
        <TodoApp viewer={data.viewer} />
    );
}

function AppRoot(props) {
    return (
        <RelayEnvironmentProvider environment={RelayEnvironment}>
            <Suspense fallback={'Loading...'}>
                <App preloadedQuery={preloadedQuery} />
            </Suspense>
        </RelayEnvironmentProvider>
    );
}

export default AppRoot;
