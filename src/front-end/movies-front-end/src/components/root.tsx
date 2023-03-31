import { FC, Suspense } from "preact/compat";
import { Outlet } from "react-router-dom";
import Header from "./header/header";
import React from "react";
import { Spinner } from "flowbite-react";

const Root: FC = () => {

  return (
    <>
      <Header/>
      <Suspense fallback={
        <div className="flex justify-center flex-row mt-8">
          <Spinner size="xl"/>
        </div>
      }>
        <Outlet/>
      </Suspense>
    </>
  );
}

export default Root;
