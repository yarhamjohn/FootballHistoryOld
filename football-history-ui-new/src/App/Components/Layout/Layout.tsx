import { FC, ReactElement, ReactNode } from "react";
import { AppHeader } from "../AppHeader/AppHeader";

type Props = {
  activeTab: number;
  setActiveTab: (index: number) => void;
  children?: ReactNode;
};

const Layout: FC<Props> = ({ activeTab, setActiveTab, children }): ReactElement => {
  return (
    <>
      <AppHeader activeTab={activeTab} setActiveTab={setActiveTab} />
      {children}
    </>
  );
};

export { Layout };
