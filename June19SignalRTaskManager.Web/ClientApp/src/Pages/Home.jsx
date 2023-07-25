import React, { useState, useEffect, useRef } from 'react';
import { Link } from 'react-router-dom';
import axios from 'axios';
import { useAuth } from '../AuthContext';
import { HubConnectionBuilder } from '@microsoft/signalr';

const Home = () => {
    const { user } = useAuth();
    const connectionRef = useRef(null);

    const [tasks, setTasks] = useState([]);
    const [newTaskTitle, setNewTaskTitle] = useState();

    useEffect(() => {
        const connectToHub = async () => {
            const connection = new HubConnectionBuilder().withUrl("/api/test").build();
            await connection.start();
            connection.invoke('newUser');
            connectionRef.current = connection;

            connection.on('taskAdded', task => {
                setTasks(tasks => [...tasks, task]);
            });

            connection.on('taskDeleted', taskId => {
                setTasks(tasks => tasks.filter(task => task.id !== taskId));
            });

            connection.on('taskStatusChanged', tasks => {
                setTasks(tasks);
            });
        }
       
        connectToHub();
        getTasks();
    }, []);

    const addTask = async () => {
        await axios.post(`/api/task/addtask?tasktitle=${newTaskTitle}`);
    }

    const getTasks = async () => {
        const { data } = await axios.get(`/api/task/getalltasks`);
        setTasks(data);
    }

    const onImDoingThisOneClick = async (taskId) => {
        await axios.post(`/api/task/adduseridtotask?taskId=${taskId}`);
    }

    const onImDoneClick = async (taskId) => {
        await axios.post(`/api/task/deletetask?taskId=${taskId}`);
    }

    return (
        <div className="container" style={{ marginTop: 80 }}>
            <div style={{ marginTop: 70 }}>
                <div className="row">
                    <div className="col-md-10">
                        <input
                            type="text"
                            className="form-control"
                            placeholder="Task Title"
                            defaultValue=""
                            value={newTaskTitle}
                            onChange={e => setNewTaskTitle(e.target.value)}
                        />
                    </div>
                    <div className="col-md-2">
                        <button onClick={addTask} className="btn btn-primary w-100">Add Task</button>
                    </div>
                </div>
                <table className="table table-hover table-striped table-bordered mt-3">
                    <thead>
                        <tr>
                            <th>Title</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        {tasks.map(task =>
                            <tr key={task.id}>
                                <td>{task.title}</td>
                                <td>
                                    {!task.userId ? <button onClick={() => onImDoingThisOneClick(task.id)} className="btn btn-dark">I'm doing this one!</button> :
                                        task.userId === user.id ?
                                            <td><button onClick={() => onImDoneClick(task.id)} className="btn btn-success">I'm done!</button></td> :
                                            <button className="btn btn-warning" disabled="">{task.user.firstName} {task.user.lastName} is doing this</button>}
                                </td>
                            </tr>
                        )}

                    </tbody>
                </table>
            </div>
        </div>

    )

}

export default Home;

