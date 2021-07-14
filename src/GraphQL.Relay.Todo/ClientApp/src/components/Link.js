import React from "react";
import PropTypes from "prop-types";
import classnames from "classnames";
import { useStore } from "laco-react";
import { TodoStore, setVisibilityFilter } from "../stores/todo";

const Link = ({ children, filter }) => {
    const { visibilityFilter } = useStore(TodoStore);

    return (
        <a
            className={classnames({ selected: filter === visibilityFilter })}
            style={{ cursor: "pointer" }}
            onClick={() => setVisibilityFilter(filter)}
        >
            {children}
        </a>
    );
};

Link.propTypes = {
    children: PropTypes.node.isRequired,
    filter: PropTypes.string.isRequired
};

export default Link;
