import React from "react";
import { Outlet } from "react-router-dom";
import { VestimentaProvider } from "../context/VestimentaContext";
import Footer from "./Footer";
import Navbar from "./Navbar";
import RightSidebar from "./RightSidebar";
import Sidebar from "./Sidebar";

const MainTemplate = _ => {
    return (
        <div className="App">
            <VestimentaProvider>
                <div className='container-fluid'>
                    <Navbar />
                    <div className="container-fluid page-body-wrapper">
                        <RightSidebar />
                        <Sidebar />
                        <div className="main-panel">
                            <div className="content-wrapper">
                                <Outlet />
                            </div>
                        </div>
                    </div>
                    <Footer />
                </div>
            </VestimentaProvider>
        </div>
    )
}

export default MainTemplate;