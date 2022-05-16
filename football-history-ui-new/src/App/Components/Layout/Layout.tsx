import { FC, ReactElement, ReactNode } from "react";
import { AppHeader } from "../AppHeader/AppHeader";

type Props = {
  children?: ReactNode;
};

const Layout: FC<Props> = ({ children }): ReactElement => {
  return (
    <>
      <AppHeader />
      {children}
    </>
  );
};

export { Layout };
